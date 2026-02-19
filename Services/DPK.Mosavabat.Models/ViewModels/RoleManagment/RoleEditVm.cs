using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Models.ViewModels.RoleManagment;

public class RoleEditVm
{
    public int? Id { get; set; }

    [Required]
    public string DisplayName { get; set; }

    public bool IsDefault { get; set; }
}