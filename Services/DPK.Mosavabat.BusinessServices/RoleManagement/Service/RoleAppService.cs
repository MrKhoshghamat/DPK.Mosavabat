using System.Threading.Tasks;
using DPK.Foundation.Application.Services.Dto;
using DPK.Mosavabat.BusinessServices.RoleManagement.Contract;
using DPK.Mosavabat.Models.EntitiesDto.RoleManagment;
using DPK.Mosavabat.Models.ViewModels.RoleManagment;

namespace DPK.Mosavabat.BusinessServices.RoleManagement.Service;

/// <summary>
/// Role app service implementation
/// </summary>
public class RoleAppService : ManagementServiceBase, IRoleAppService
{
    /// <summary>
    /// Get roles
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<ListResultDto<RoleListDto>> GetRoles(GetRolesVm input)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get role for edit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<GetRoleForEditDto> GetRoleForEdit(NullableIdDto input)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create or update role
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task CreateOrUpdateRole(CreateOrUpdateRoleVm input)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete role
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task DeleteRole(EntityDto input)
    {
        throw new NotImplementedException();
    }
}