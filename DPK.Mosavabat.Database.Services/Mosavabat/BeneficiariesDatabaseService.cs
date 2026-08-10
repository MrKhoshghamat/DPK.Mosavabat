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
    public class BeneficiariesDatabaseService : MainDatabaseService, IBeneficiariesDatabaseService
    {
        private readonly ILoggerManager _logger;
        public BeneficiariesDatabaseService(
           IConfiguration configuration,
           ILoggerManager logger) : base(configuration)
        {
            _logger = logger;
        }


        public async Task<GetBeneficiariesLiveTotalDebtDto> GetBeneficiariesLiveTotalDebt(GetBeneficiariesLiveTotalDebtVm vm, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@NationalCode", SqlDbType.VarChar) {Value = vm.NationalCode}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[CalculateBeneficiariesLiveTotalDebt]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetBeneficiariesLiveTotalDebtDto() { BeneficiariesLiveTotalDebt = Convert.ToInt64(result!.Result!) };
        }
    }
}
