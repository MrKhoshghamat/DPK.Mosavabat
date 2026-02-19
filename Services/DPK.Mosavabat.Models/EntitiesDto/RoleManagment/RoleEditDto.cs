using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Models.EntitiesDto.RoleManagment;

public class RoleEditDto
{
    public int? Id { get; set; }

    [Required]
    public string DisplayName { get; set; }

    public bool IsDefault { get; set; }
}