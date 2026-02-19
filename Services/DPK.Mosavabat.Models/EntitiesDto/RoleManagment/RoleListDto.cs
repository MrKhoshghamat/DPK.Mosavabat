using System;
using DPK.Foundation.Application.Services.Dto;
using DPK.Foundation.Domain.Entities.Auditing;

namespace DPK.Mosavabat.Models.EntitiesDto.RoleManagment;

public class RoleListDto : EntityDto, IHasCreationTime
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public bool IsStatic { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreationTime { get; set; }
}