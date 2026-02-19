using DPK.Foundation.Application.Services.Dto;

namespace DPK.Mosavabat.Models.EntitiesDto.Account;

public class OrganizationUnitDto : AuditedEntityDto<long>
{
    public long? ParentId { get; set; }

    public string Code { get; set; }

    public string DisplayName { get; set; }

    public int MemberCount { get; set; }

    public int RoleCount { get; set; }
}