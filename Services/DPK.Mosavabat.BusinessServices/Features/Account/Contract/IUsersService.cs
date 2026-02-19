using System.Collections.Generic;
using DPK.Foundation.Application.Services.Dto;
using DPK.Mosavabat.Models.EntitiesDto.Account;
using DPK.Mosavabat.Models.ViewModels.Account;
using System.Threading.Tasks;

namespace DPK.Mosavabat.BusinessServices.Features.Account.Contract;

/// <summary>
/// User service interface
/// </summary>
public interface IUsersService : IApplicationService
{
    /// <summary>
    /// Get users
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IEnumerable<UserListDto>> GetUsers(GetUsersVm input);

    /// <summary>
    /// Get users for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GetUserForEditDto> GetUserForEdit(NullableIdDto<long> input);

    /// <summary>
    /// Get user permissions for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GetUserPermissionsForEditDto> GetUserPermissionsForEdit(EntityDto<long> input);

    /// <summary>
    /// Update user permissions
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateUserPermissions(UpdateUserPermissionsVm input);

    /// <summary>
    /// Create or update user
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateOrUpdateUser(CreateOrUpdateUserVm input);

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteUser(EntityDto<long> input);

    /// <summary>
    /// Unlock user
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UnlockUser(EntityDto<long> input);
}