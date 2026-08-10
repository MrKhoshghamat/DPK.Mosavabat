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
    public class RelatedPersonsDatabaseService : MainDatabaseService, IRelatedPersonsDatabaseService
    {
        private readonly ILoggerManager _logger;

        public RelatedPersonsDatabaseService(
            IConfiguration configuration,
            ILoggerManager logger) : base(configuration)
        {
            _logger = logger;
        }


        public async Task<IsRelatedPersonDto> IsRelatedPersonAsync(IsRelatedPersonVm vm, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@NationalCode", SqlDbType.VarChar) {Value = vm.NationalCode}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[IsRelatedPerson]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new IsRelatedPersonDto() { IsRelated = Convert.ToBoolean(result!.Result!) };
        }

        public async Task<GetTotalDebtOfRelatedPersonsDto> GetTotalDebtOfRelatedPersons(CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>();

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm() 
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[GetTotalDebtOfRelatedPersons]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));
            return new GetTotalDebtOfRelatedPersonsDto() { TotalDebtOfRelatedPerson = result!.Result!.ToString() };
        }

    }
}
