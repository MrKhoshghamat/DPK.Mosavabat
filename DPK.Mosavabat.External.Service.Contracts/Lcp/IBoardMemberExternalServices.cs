using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp
{
    public interface IBoardMemberExternalServices
    {
        Task<List<GetBoardMemberDto>> GetBoardMembersList(GetBoardMemberDataModel getBoardMemberDataModel);
    }
}
