using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat.General;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class BizagiDatabaseService : MainDatabaseService, IBizagiDatabaseService
{
    private readonly ILoggerManager _logger;
    public BizagiDatabaseService(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<List<CaseForAbortCaseDto>?> ExtractAllCasesForAbortCase(CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>();

        var result = await ExecuteAsListAsync<CaseForAbortCaseDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[BS].[ExtractAllCasesForAbortCase]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return result.List;
    }

    public async Task<IsWfUserDto> IsWfUser(IsWfUserVm vm, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
            {
                new("@PersonnelCode", SqlDbType.VarChar) {Value = vm.PersonnelCode}
            };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[IsWfUser]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new IsWfUserDto() { Result = Convert.ToBoolean(result!.Result!) };
    }

    public async Task<HasSupportRoleDto> HasSupportRole(HasSupportRoleVm vm, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@username", SqlDbType.VarChar) {Value = vm.PersonnelCode}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[HasSupportRole]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new HasSupportRoleDto() { Result = Convert.ToBoolean(result!.Result!) };
    }
}