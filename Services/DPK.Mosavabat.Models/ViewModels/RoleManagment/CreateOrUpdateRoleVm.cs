using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Models.ViewModels.RoleManagment;

public class CreateOrUpdateRoleVm
{
    [Required]
    public RoleEditVm Role { get; set; }

    [Required]
    public List<string> GrantedPermissionNames { get; set; }
}