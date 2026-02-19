using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Models.ViewModels.Account;

public class UpdateUserPermissionsVm
{
    [Range(1, int.MaxValue)]
    public long Id { get; set; }

    [Required]
    public List<string> GrantedPermissionNames { get; set; }
}