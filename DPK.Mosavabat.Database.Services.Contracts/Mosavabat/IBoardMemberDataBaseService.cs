using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Database.Services.Contracts.Mosavabat
{
    public interface IBoardMemberDataBaseService : IMainDatabaseService
    {
        Task<List<GetBoardMembersDto>> GetBoardMembersAccounts(GetBoardMembersVm vm,CancellationToken cancellationToken);
    }
}
