using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services.Mosavabat
{
    public class BoardMemberDataBaseService : MainDatabaseService,IBoardMemberDataBaseService
    {
        private readonly ILoggerManager _logger;
        public BoardMemberDataBaseService(
            ILoggerManager logger,
            IConfiguration configuration) : base(configuration)
        {
            _logger = logger;
        }

        public async Task<List<GetBoardMembersDto>> GetBoardMembersAccounts(GetBoardMembersVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new ("@CustomerNumbers",SqlDbType.NVarChar) {Value = viewModel.CustomerNumbers}
            };

            var result = await ExecuteAsListAsync<GetBoardMembersDto>(new ExecuteAsListAsyncVm()
            {
                Parameters = parameters,
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[GetBoardMembersAccounts]"
            },
            cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;

        }
    }
}
