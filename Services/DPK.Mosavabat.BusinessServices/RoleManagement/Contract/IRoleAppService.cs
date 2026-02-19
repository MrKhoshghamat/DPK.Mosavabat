using System.Threading.Tasks;
using DPK.Foundation.Application.Services.Dto;
using DPK.Mosavabat.Models.EntitiesDto.RoleManagment;
using DPK.Mosavabat.Models.ViewModels.RoleManagment;

namespace DPK.Mosavabat.BusinessServices.RoleManagement.Contract;

/// <summary>
/// Role app service interface
/// </summary>
public interface IRoleAppService : IApplicationService
{
    /// <summary>
    /// Get roles
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<RoleListDto>> GetRoles(GetRolesVm input);

    /// <summary>
    /// Get role for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GetRoleForEditDto> GetRoleForEdit(NullableIdDto input);

    /// <summary>
    /// Create or update role
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateOrUpdateRole(CreateOrUpdateRoleVm input);

    /// <summary>
    /// Delete role
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteRole(EntityDto input);
}