using DPK.Mosavabat.External.Services.Contracts.Lcp;
using IServiceProvider = DPK.Mosavabat.External.Services.Contracts.Lcp.IServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lcp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DPK.Mosavabat.External.Services.Lcp
{
    public class BoardMemberServices : IBoardMemberExternalServices
    {
        private readonly IServiceProvider _serviceProvider;
        public BoardMemberServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public async Task<List<GetBoardMemberDto>> GetBoardMembersList(GetBoardMemberDataModel getBoardMemberDataModel)
        {
            var response = await _serviceProvider.GetBoardMembersListService(getBoardMemberDataModel);
            var getBoardMemberDtoList = new List<GetBoardMemberDto>();
            var responseCount = JObject.Parse(response)["result"].ToList().Count();
            if (responseCount > 0 )
            {
                var boardMember = JObject.Parse(response)["result"]["boardMemberItems"].Select(x => (string)x["customerNumber"]).ToList();

                foreach (var item in boardMember)
                {
                    getBoardMemberDtoList.Add(new GetBoardMemberDto
                    {
                        CustomerNumber = item.PadLeft(10,'0'),
                    });
                }
            } 

            return getBoardMemberDtoList;
        }
    }
}
