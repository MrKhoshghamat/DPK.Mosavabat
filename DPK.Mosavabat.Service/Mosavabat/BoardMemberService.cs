using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Lcp;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPK.Mosavabat.Services.Mosavabat
{
    public class BoardMemberService : IBoardMemberService
    {
        private readonly IBoardMemberDataBaseService _dataBaseServices;
        private readonly IBoardMemberExternalServices _externalServices;
        public BoardMemberService(
            IBoardMemberDataBaseService dataBaseServices,
            IBoardMemberExternalServices externalServices)
        {
            _dataBaseServices = dataBaseServices; 
            _externalServices = externalServices;
        }


        public async Task <List<GetBoardMembersDto>> GetBoardMembersList(GetBoardMemberDataModel dataModel,CancellationToken cancellationToken)
        {
            var customerNumbers = await _externalServices.GetBoardMembersList(dataModel);
            var customerNumbersString = string.Join(",", customerNumbers.Select(x => x.CustomerNumber));
            var boardMembers =  await _dataBaseServices.GetBoardMembersAccounts(new GetBoardMembersVm() { CustomerNumbers = customerNumbersString },cancellationToken);
            return boardMembers;
        }

    }
}
