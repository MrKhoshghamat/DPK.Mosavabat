using DPK.Mosavabat.Models.EntitiesDto.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Models.ViewModels.Account;

public class CreateOrUpdateUserVm
{
    [Required]
    public UserEditVm User { get; set; }

    [Required]
    public string[] AssignedRoleNames { get; set; }

    public bool SendActivationEmail { get; set; }

    public bool SetRandomPassword { get; set; }

    public List<long> OrganizationUnits { get; set; }

    public CreateOrUpdateUserVm()
    {
            OrganizationUnits = new List<long>();
        }
}