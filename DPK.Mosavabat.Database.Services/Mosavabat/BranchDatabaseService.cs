using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat
{
    public class BranchDatabaseService : MainDatabaseService, IBranchDatabaseService
    {
        private readonly ILoggerManager _logger;
        public BranchDatabaseService(IConfiguration configuration, ILoggerManager logger) : base(configuration)
        {
            _logger = logger;
        }

        public async Task<GetBranchCodeDto> GetBranchCode(GetBranchCodeVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
                {new("@idLcpRequestFacility", SqlDbType.Int) {Value = viewModel.IdLcpRequestFacility}};

            var branchCode =
                await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[LCP].[ExtractBranchCode]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(branchCode));

            return new GetBranchCodeDto() { BranchCode = branchCode?.Result?.ToString() };
        }

        public async Task<GetBranchCodeDto> GetBranchCode(GetBranchCodeFromPaymentPermissionByC13Vm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
                {new("@C13", SqlDbType.NVarChar) {Value = viewModel.C13}};

            var branchCode =
                await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[CP].[GetBranchFromPaymentPermissionByC13]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(branchCode));

            return new GetBranchCodeDto() { BranchCode = branchCode.Result?.ToString() };
        }
    }
}
