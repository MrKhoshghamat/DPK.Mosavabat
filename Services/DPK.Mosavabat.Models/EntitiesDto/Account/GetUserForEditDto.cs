using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Models.EntitiesDto.Account;

public class GetUserForEditDto
{
    public Guid? ProfilePictureId { get; set; }

    public UserEditDto User { get; set; }

    public UserRoleDto[] Roles { get; set; }

    public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

    public List<string> MemberedOrganizationUnits { get; set; }
}