using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class ContractDatabaseService : MainDatabaseService, IContractDatabaseService
{
    private readonly ILoggerManager _logger;
    public ContractDatabaseService(IConfiguration configuration, ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<GetContractTypeTitleByContractTypeIdDto> GetContractTypeTitleByContractTypeId(GetContractTypeTitleByContractTypeIdDm dataModel, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@contractTypeId", SqlDbType.Int) {Value = dataModel.ContractTypeId},
        };

        var contractTypeTitle = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[GetContractTypeTitleByContractTypeId]",
            Parameters = parameters
        }, cancellationToken);

        return new GetContractTypeTitleByContractTypeIdDto()
        {
            ContractTypeTitle = contractTypeTitle.Result.ToString()
        };
    }

    public async Task<GetSubContractTypeTitleBySubContractTypeIdDto> GetSubContractTypeTitleBySubContractTypeId(GetSubContractTypeTitleBySubContractTypeIdDm dataModel, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@subContractTypeId", SqlDbType.Int) {Value = dataModel.SubContractTypeId},
        };

        var subContractTypeTitle = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[GetSubContractTypeTitleBySubContractTypeId]",
            Parameters = parameters
        }, cancellationToken);

        return new GetSubContractTypeTitleBySubContractTypeIdDto()
        {
            SubContractTypeTitle = subContractTypeTitle.Result.ToString()
        };
    }

    public async Task UpdateCuCustomerInCuContract(UpdateCuCustomerInCuContractDm dataModel, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@IdCu_Customer", SqlDbType.Int) {Value = dataModel.IdCu_Customer},
            new("@C13", SqlDbType.VarChar) {Value = dataModel.C13},
            new("@C16", SqlDbType.VarChar) {Value = dataModel.C16}
        };

        var rowAffected = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[UpdateCuCustomerInCuContract]",
            Parameters = parameters
        }, cancellationToken);
    }

    public async Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(UpdateIsDeletedCuContractDm dataModel, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>
        {
            new("@PaymentPermissionId", SqlDbType.Int) {Value = dataModel.PaymentPermissionId},
            new("@IsDeleted", SqlDbType.Bit) {Value = dataModel.IsDeleted},
        };

        var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[dbo].[UpdateIsDeletedCuContract]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateIsDeletedCuContractDto() { IsUpdated = Convert.ToBoolean(result.Result) };
    }
}
