using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services.Mosavabat
{
    public class ContractGroupDatabaseService : MainDatabaseService, IContractGroupDatabaseService
    {
        private readonly ILoggerManager _logger;
        public ContractGroupDatabaseService(
            IConfiguration configuration,
            ILoggerManager logger) : base(configuration)
        {
            _logger = logger;
        }

        public async Task<ExtractContractGroupIdDto?> ExtractContractGroupId(ExtractContractGroupIdVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {new("@contractGroupCode", SqlDbType.Int) {Value = viewModel.ContractGroupCode}};

            var result =
                await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[LCP].[ExtractContractGroupId]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new ExtractContractGroupIdDto() { ContractGroupId = result.Result?.ToString() };
        }
    }
}
