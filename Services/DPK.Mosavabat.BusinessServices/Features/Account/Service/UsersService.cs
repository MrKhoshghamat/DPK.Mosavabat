using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DPK.Foundation.Authorization;
using DPK.Foundation.Authorization.Users;
using DPK.Foundation.Configuration;
using DPK.Foundation.Domain.Repositories;
using DPK.Foundation.Identity.Models.Core;
using DPK.Mosavabat.BusinessServices.Features.Account.Contract;
using DPK.Mosavabat.Models.EntitiesDto.Account;
using DPK.Mosavabat.Models.ViewModels.Account;
using DPK.Foundation.Application.Services.Dto;
using DPK.Foundation.Collections.Extensions;
using DPK.Foundation.Dependency;
using DPK.Foundation.Identity.Extentions;
using DPK.Foundation.Identity.Services.Roles;
using DPK.Foundation.Identity.Services.Users;
using DPK.Foundation.Organizations;
using DPK.Foundation.Identity.Extentions.IdentityExtentions;
using DPK.Mosavabat.GeneralServices.Account.Contract;
using Microsoft.AspNetCore.Identity;
using DPK.Foundation.Notifications;
using DPK.Foundation.Runtime.Session;
using DPK.Foundation.UI;
using DPK.Mosavabat.GeneralServices.Account.Service;
using Microsoft.EntityFrameworkCore;
using DPK.Foundation.Extensions;

namespace DPK.Mosavabat.BusinessServices.Features.Account.Service;

/// <summary>
/// Users services implementations
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="roleRepository"></param>
/// <param name="permissionRepository"></param>
/// <param name="userRoleRepository"></param>
/// <param name="roleManagementConfig"></param>
/// <param name="organizationUnitRepository"></param>
/// <param name="userOrganizationUnitRepository"></param>
/// <param name="organizationUnitRoleRepository"></param>
/// <param name="passwordHasher"></param>
/// <param name="notificationSubscriptionManager"></param>
/// <param name="passwordValidators"></param>
/// <param name="userEmailer"></param>
/// <param name="userManager"></param>
/// <param name="roleManager"></param>
public class UsersService(
    IRepository<Role> roleRepository,
    IRepository<PermissionSetting, long> permissionRepository,
    IRepository<UserRole, long> userRoleRepository,
    IRoleManagementConfig roleManagementConfig,
    IRepository<OrganizationUnit, long> organizationUnitRepository,
    IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
    IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
    IPasswordHasher<User> passwordHasher,
    INotificationSubscriptionManager notificationSubscriptionManager,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    IUserEmailer userEmailer,
    UserManager userManager,
    RoleManager roleManager) : ManagementServiceBase, IUsersService
{
    /// <summary>
    /// App url service
    /// </summary>
    public IAppUrlService AppUrlService { get; set; } = NullAppUrlService.Instance;


    /// <summary>
    /// Get users
    /// </summary>
    /// <param name="vm"></param>
    /// <returns></returns>
    public async Task<IEnumerable<UserListDto>> GetUsers(GetUsersVm vm)
    {
        using var component = IocManager.Instance.ResolveAsDisposable<IGridComponent>();
        var query = GetUsersFilteredQuery(vm);
        await query.CountAsync();
        return ObjectMapper.Map<IEnumerable<UserListDto>>(query);
    }

    /// <summary>
    /// Get users for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<GetUserForEditDto> GetUserForEdit(NullableIdDto<long> input)
    {
        //Getting all available roles
        var userRoleDtos = await roleManager.Roles
            .OrderBy(r => r.DisplayName)
            .Select(r => new UserRoleDto
            {
                RoleId = r.Id,
                RoleName = r.Name,
                RoleDisplayName = r.DisplayName
            })
            .ToArrayAsync();

        var allOrganizationUnits = await organizationUnitRepository.GetAllListAsync();

        var output = new GetUserForEditDto
        {
            Roles = userRoleDtos,
            AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
            MemberedOrganizationUnits = []
        };

        if (!input.Id.HasValue)
        {
            //Creating a new user
            output.User = new UserEditDto
            {
                IsActive = true,
                ShouldChangePasswordOnNextLogin = true,
                IsTwoFactorEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(DpkPrimitiveSettingNames.UserManagement
                        .TwoFactorLogin.IsEnabled),
                IsLockoutEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(DpkPrimitiveSettingNames.UserManagement.UserLockOut
                        .IsEnabled)
            };

            foreach (var defaultRole in await roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
            {
                var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                if (defaultUserRole != null)
                {
                    defaultUserRole.IsAssigned = true;
                }
            }
        }
        else
        {
            //Editing an existing user
            var user = await userManager.GetUserByIdAsync(input.Id.Value);

            output.User = ObjectMapper.Map<UserEditDto>(user);
            //output.ProfilePictureId = user.SignInToken.ProfilePictureId;

            var organizationUnits = await userManager.GetOrganizationUnitsAsync(user);
            output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();

            var allRolesOfUsersOrganizationUnits = GetAllRoleNamesOfUsersOrganizationUnits(input.Id.Value);

            foreach (var userRoleDto in userRoleDtos)
            {
                userRoleDto.IsAssigned = await userManager.IsInRoleAsync(user, userRoleDto.RoleName);
                userRoleDto.InheritedFromOrganizationUnit =
                    allRolesOfUsersOrganizationUnits.Contains(userRoleDto.RoleName);
            }
        }

        return output;
    }

    /// <summary>
    /// Get user permissions for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<GetUserPermissionsForEditDto> GetUserPermissionsForEdit(EntityDto<long> input)
    {
        var user = await userManager.GetUserByIdAsync(input.Id);
        var permissions = PermissionManager.GetAllPermissions();
        var grantedPermissions = await userManager.GetGrantedPermissionsAsync(user);

        List<FlatPermissionDto> list = [.. ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName)];
        return new GetUserPermissionsForEditDto
        {
            Permissions = list,
            GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
        };
    }

    /// <summary>
    /// Update user permissions
    /// </summary>
    /// <param name="input"></param>
    public async Task UpdateUserPermissions(UpdateUserPermissionsVm input)
    {
        var user = await userManager.GetUserByIdAsync(input.Id);
        var grantedPermissions =
            PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
        await userManager.SetGrantedPermissionsAsync(user, grantedPermissions);
    }

    /// <summary>
    /// Create or update user
    /// </summary>
    /// <param name="input"></param>
    public async Task CreateOrUpdateUser(CreateOrUpdateUserVm input)
    {
        if (input.User.Id.HasValue)
        {
            await UpdateUserAsync(input);
        }
        else
        {
            await CreateUserAsync(input);
        }
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="input"></param>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task DeleteUser(EntityDto<long> input)
    {
        if (input.Id == DpkSession.GetUserId())
        {
            throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
        }

        var user = await userManager.GetUserByIdAsync(input.Id);
        CheckErrors(await userManager.DeleteAsync(user));
    }

    /// <summary>
    /// Unlock user
    /// </summary>
    /// <param name="input"></param>
    public async Task UnlockUser(EntityDto<long> input)
    {
        var user = await userManager.GetUserByIdAsync(input.Id);
        user.Unlock();
    }


    #region Private Functionality

    private IQueryable<User> GetUsersFilteredQuery(IGetUsersVm input)
    {
        var query = userManager.Users.AsEnumerable()
            .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role))
            .WhereIf(input.OnlyLockedUsers,
                u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(input.Filter) ||
                    u.Surname.Contains(input.Filter) ||
                    u.UserName.Contains(input.Filter) ||
                    u.EmailAddress.Contains(input.Filter)
            );

        if (input.Permissions == null || input.Permissions.All(p => p.IsNullOrWhiteSpace())) return (IQueryable<User>)query;
        {
            var staticRoleNames = roleManagementConfig.StaticRoles.Where(
                r => r.GrantAllPermissionsByDefault &&
                     r.Side == DpkSession.MultiTenancySide
            ).Select(r => r.RoleName).ToList();

            input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

            var userIds = from user in query
                          join ur in userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                          from ur in urJoined.DefaultIfEmpty()
                          join urr in roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                          from urr in urrJoined.DefaultIfEmpty()
                          join up in permissionRepository.GetAll()
                                  .Where(userPermission => input.Permissions.Contains(userPermission.Name))
                              on user.Id equals up.UserId into upJoined
                          from up in upJoined.DefaultIfEmpty()
                          join rp in permissionRepository.GetAll()
                                  .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                              new { RoleId = ur?.RoleId ?? 0 } equals new { RoleId = (int)rp.RoleId } into rpJoined
                          from rp in rpJoined.DefaultIfEmpty()
                          where up is { IsGranted: true } ||
                                (up == null && rp is { IsGranted: true }) ||
                                (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                          group user by user.Id
                into userGrouped
                          select userGrouped.Key;

            query = userManager.Users.Where(e => userIds.Contains(e.Id));
        }

        return (IQueryable<User>)query;
    }

    private List<string> GetAllRoleNamesOfUsersOrganizationUnits(long userId)
    {
        var list = new List<string>();
        foreach (var s in (userOrganizationUnitRepository.GetAll()
                     .Join(organizationUnitRoleRepository.GetAll(), userOu => userOu.OrganizationUnitId,
                         roleOu => roleOu.OrganizationUnitId, (userOu, roleOu) => new { userOu, roleOu })
                     .Join(roleRepository.GetAll(), @t => @t.roleOu.RoleId, userOuRoles => userOuRoles.Id,
                         (@t, userOuRoles) => new { @t, userOuRoles }).Where(@t => @t.@t.userOu.UserId == userId)
                     .Select(@t => @t.userOuRoles.Name))) list.Add(s);
        return list;
    }


    private async Task UpdateUserAsync(CreateOrUpdateUserVm input)
    {
        Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

        var user = await userManager.FindByIdAsync(input.User.Id.Value.ToString());

        //Update user properties
        ObjectMapper.Map(input.User, user); //Passwords are not mapped (see mapping configuration)

        if (user != null)
        {
            CheckErrors(await userManager.UpdateAsync(user));

            if (input.SetRandomPassword)
            {
                var randomPassword = await userManager.CreateRandomPassword();
                user.Password = passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await userManager.InitializeOptionsAsync(DpkSession.TenantId);
                CheckErrors(await userManager.ChangePasswordAsync(user, input.User.Password));
            }

            //Update roles
            CheckErrors(await userManager.SetRolesAsync(user, input.AssignedRoleNames));

            //update organization units
            await userManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(DpkSession.TenantId),
                    input.User.Password
                );
            }
        }
    }
    private async Task CreateUserAsync(CreateOrUpdateUserVm input)
    {


        var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
        user.TenantId = DpkSession.TenantId;

        //Set password
        if (input.SetRandomPassword)
        {
            var randomPassword = await userManager.CreateRandomPassword();
            user.Password = passwordHasher.HashPassword(user, randomPassword);
            input.User.Password = randomPassword;
        }
        else if (!input.User.Password.IsNullOrEmpty())
        {
            await userManager.InitializeOptionsAsync(DpkSession.TenantId);
            foreach (var validator in passwordValidators)
            {
                CheckErrors(await validator.ValidateAsync(userManager, user, input.User.Password));
            }

            user.Password = passwordHasher.HashPassword(user, input.User.Password);
        }

        user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

        //Assign roles
        user.Roles = [];
        foreach (var roleName in input.AssignedRoleNames)
        {
            var role = await roleManager.GetRoleByNameAsync(roleName);
            user.Roles.Add(new UserRole(DpkSession.TenantId, user.Id, role.Id));
        }

        CheckErrors(await userManager.CreateAsync(user));
        await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

        //Notifications
        await notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());

        //Organization Units
        await userManager.SetOrganizationUnitsAsync(user, [.. input.OrganizationUnits]);

        //Send activation email
        if (input.SendActivationEmail)
        {
            user.SetNewEmailConfirmationCode();
            await userEmailer.SendEmailActivationLinkAsync(
                user,
                AppUrlService.CreateEmailActivationUrlFormat(DpkSession.TenantId),
                input.User.Password
            );
        }
    }

    #endregion
}