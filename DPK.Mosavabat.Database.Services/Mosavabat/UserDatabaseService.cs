using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat
{
    public class UserDatabaseService : MainDatabaseService, IUserDatabaseService
    {
        private readonly IBranchDatabaseService _branchDatabaseService;
        private readonly ILoggerManager _logger;
        public UserDatabaseService(
            IConfiguration configuration,
            IBranchDatabaseService branchDatabaseService,
            ILoggerManager logger) : base(configuration)
        {
            _branchDatabaseService = branchDatabaseService;
            _logger = logger;
        }

        public async Task<GetCustomerEkDto> GetCustomerEk(GetCustomerEkVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new ("@AccountNo", SqlDbType.NVarChar) {Value = viewModel.AccountNo}
            };

            var idCustomerEk = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[GetCustomerEk]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(idCustomerEk));

            return new GetCustomerEkDto() { IdCuCustomerEk = Convert.ToInt32(idCustomerEk.Result) };
        }

        public async Task<GetFacilityManagerOrSupervisorDto> GetFacilityManagerOrSupervisor(GetFacilityManagerOrSupervisorVm getFacilityManagerOrSupervisorVm,
            CancellationToken cancellationToken)
        {
            var branchCode = await _branchDatabaseService.GetBranchCode(new GetBranchCodeVm() { IdLcpRequestFacility = getFacilityManagerOrSupervisorVm.IdLcpRequestFacility }, cancellationToken);

            var parameters = new List<SqlParameter>
            {
                new("@branchCode", SqlDbType.Int) {Value = branchCode}
            };

            var username = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[LCP].[FindFacilityManagerOrSupervisor]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(username));

            return new GetFacilityManagerOrSupervisorDto() { Username = username.ToString() };
        }

        public async Task<GetUserNameDto> GetUserName(GetUserNameVm getUserNameVm, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@branchCode", SqlDbType.Int) {Value = getUserNameVm.BranchCode}
            };

            var username = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = getUserNameVm.MosavabatClassName == MosavabatClassName.AgentRegistry ? "[LCP].[FindBranchManagerOrSupervisor]" : "[LCP].[FindFacilityManagerOrSupervisor]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(username));

            return new GetUserNameDto() { Username = username.Result.ToString() };
        }


    }
}
