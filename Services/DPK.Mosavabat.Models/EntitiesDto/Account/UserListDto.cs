using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPK.Foundation.Application.Services.Dto;
using DPK.Foundation.Domain.Entities;
using DPK.Foundation.Domain.Entities.Auditing;
using DPK.Mosavabat.Models.EntitiesDto.RoleManagment;

namespace DPK.Mosavabat.Models.EntitiesDto.Account;

public class UserListDto : EntityDto<long>, IPassivable, IHasCreationTime
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string UserName { get; set; }

    public string EmailAddress { get; set; }

    public string PhoneNumber { get; set; }

    public Guid? ProfilePictureId { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public List<UserListRoleDto> Roles { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationTime { get; set; }
}