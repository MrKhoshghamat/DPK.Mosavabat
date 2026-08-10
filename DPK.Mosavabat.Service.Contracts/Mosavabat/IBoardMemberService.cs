using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Services.Contracts.Mosavabat
{
    public interface IBoardMemberService
    {
        Task<List<GetBoardMembersDto>> GetBoardMembersList(GetBoardMemberDataModel getBoardMemberDataModel,CancellationToken cancellationToken);
    }
}
