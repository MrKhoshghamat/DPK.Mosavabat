using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.EntitiesDto;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat.Contract;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class RequestFacilityDatabaseService : MainDatabaseService, IRequestFacilityDatabaseService
{
    private readonly ILoggerManager _logger;

    public RequestFacilityDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<UpdateLcpRequestFacilityDto> UpdateLcpRequestFacility(UpdateLcpRequestFacilityVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@idCase", SqlDbType.Int) {Value = viewModel.IdCase},
            new("@initialRadNumber", SqlDbType.Int) {Value = viewModel.InitialRadNumber},
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = viewModel.IdCaseApprovalRequest},
            new("@radNumberApprovalRequest", SqlDbType.Int) {Value = viewModel.RadNumberApprovalRequest},
            new("@lcpRequestFacilityId", SqlDbType.Int) {Value = viewModel.LcpRequestFacilityId},
            new("@guidLcpRequestFacility", SqlDbType.NVarChar) {Value = viewModel.GuidLcpRequestFacility}
        };

        var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[UpdateLcpRequestFacility]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateLcpRequestFacilityDto() { Result = result.ToString() };
    }

    public async Task<MapLcpRequestFacilityToApprovalRequestDto> MapLcpRequestFacilityToApprovalRequest(MapLcpRequestFacilityToApprovalRequestVm viewModel,
        CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@idLCP_RequestFacility", SqlDbType.Int) {Value = viewModel.IdLcpRequestFacility},
            new("@idCase", SqlDbType.Int) {Value = viewModel.IdCase},
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = viewModel.IdCaseApprovalRequest}
        };

        var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[MapLcpRequestFacilityToApprovalRequest]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new MapLcpRequestFacilityToApprovalRequestDto() { Result = result.ToString() };
    }

    public async Task<GetRequestFacilityCaseIdDto> GetRequestFacilityCaseId(GetRequestFacilityCaseIdVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@idCaseApprovalRequest", SqlDbType.Int) {Value = viewModel.IdCaseApprovalRequest}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetRequestFacilityCaseId]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetRequestFacilityCaseIdDto() { Result = result.ToString() };
    }

    public async Task<GetContractFileLinkDto> GetContractFileLink(GetContractFileLinkVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@contractId", SqlDbType.Int) {Value = viewModel.ContractId}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetContractFileLink]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetContractFileLinkDto() { Result = result.ToString() };
    }

    public async Task<UpdateContractFileLinkDto> UpdateContractFileLink(UpdateContractFileLinkVm viewModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@FileLink", SqlDbType.VarChar) {Value = viewModel.FileLink},
            new("@CuApprovalSubjectId", SqlDbType.VarChar) {Value = viewModel.CuApprovalSubjectId}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[UpdateContractFileLink]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateContractFileLinkDto() { Result = result.ToString() };
    }

    public async Task<GetApprovalContractsDto> GetApprovalContracts(GetApprovalContractsDm dataModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@MosavabeNo", SqlDbType.NVarChar) {Value = dataModel.MosavabeNo}
        };

        var result = await ExecuteAsListAsync<ContractDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[CP].[GetApprovalContracts]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetApprovalContractsDto() { Contracts = result.List };
    }

    public async Task<UpdateLcpRequestRadNumberDto> UpdateLcpRequestRadNumber(UpdateLcpRequestRadNumberDataModel dataModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@idApprovalRequest", SqlDbType.Int) {Value = dataModel.IdApprovalRequest},
            new("@radNumber", SqlDbType.VarChar) {Value = dataModel.RadNumber}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "LCP.UpdateLcpRequestRadNumber",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateLcpRequestRadNumberDto() { IsUpdated = Convert.ToBoolean(result.Result) };
    }

    public async Task<UpdateLcpRequestRadNumberDto> InsertReqFacilityStatus(InsertReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@idRequestFacility", SqlDbType.Int) {Value = dataModel.IdRequestFacility},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "LCP.InsertReqFacilityStatus",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateLcpRequestRadNumberDto() { IsUpdated = Convert.ToBoolean(result.Result) };
    }

    public async Task<UpdateLcpRequestRadNumberDto> UpdateReqFacilityStatus(UpdateReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));
        var parameters = new List<SqlParameter>()
        {
            new("@CaseId", SqlDbType.Int) {Value = dataModel.CaseId},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "LCP.UpdateReqFacilityStatus",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new UpdateLcpRequestRadNumberDto() { IsUpdated = Convert.ToBoolean(result.Result) };
    }

    public async Task<string?> GetApprovalRequestId(UpdateReqFacilityStatusDataModel dataModel, CancellationToken cancellationToken)
    {
        _logger.Trace(JsonConvert.SerializeObject(dataModel));

        var parameters = new List<SqlParameter>()
        {
            new("@CaseId", SqlDbType.Int) {Value = int.Parse(dataModel.CaseId)},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "LCP.GetApprovalRequestId",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        //if (result.Result is {})
        //{
        //    return null;
        //}
        return result.Result?.ToString();
    }

    public async Task<GetRadNumberByCaseIdDto> GetRadNumberByCaseId(GetRadNumberByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var parameters = new List<SqlParameter>()
        {
            new("@caseId", SqlDbType.Int) {Value = viewModel.CaseId},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "dbo.GetRadNumberByCaseId",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetRadNumberByCaseIdDto()
        {
            RadNumber = Convert.ToInt32(result.Result)
        };
    }

    public async Task<GetUserNameByCaseIdDto> GetUserNameByCaseId(GetUserNameByCaseIdVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var parameters = new List<SqlParameter>()
        {
            new("@caseId", SqlDbType.Int) {Value = viewModel.CaseId},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "dbo.GetUserNameByCaseId",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetUserNameByCaseIdDto()
        {
            UserName = result.Result!.ToString()
        };
    }

    public async Task<GetUserIdByUserNameDto> GetUserIdByUserName(GetUserIdByUserNameVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var parameters = new List<SqlParameter>()
        {
            new("@username", SqlDbType.VarChar) {Value = viewModel.UserName},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "dbo.GetUserIdByUsername",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetUserIdByUserNameDto()
        {
            UserId = Convert.ToInt32(result.Result!)
        };
    }

    public async Task LcpRequestFacilityAbortCase(int caseId, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@caseId", SqlDbType.Int) {Value = caseId}
        };

        var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "dbo.LCP_RequestFacilityAbortCase",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
    }

    public async Task<GetCustomerRankDto> GetCustomerRank(int lcpRequestFacilityId, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@lcpRequestFacilityId", SqlDbType.VarChar) {Value = lcpRequestFacilityId}
        };

        var result = await ExecuteReaderAsync(new ExecuteReaderAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetCustomerRank]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetCustomerRankDto() { CreditScore = Convert.ToSingle(result.Result), CreditRank = result.Column2!.ToString() };
    }

    public async Task<List<GetCustomerDebtDetailsDto>> GetCustomerDebtDetails(GetCustomerDebtDetailsDataModel dataModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@CustomerNumber" , SqlDbType.VarChar) {Value = dataModel.CustomerNumber}
        };


        var result = await ExecuteAsListAsync<GetCustomerDebtDetailsDto>(new ExecuteAsListAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetCustomerDebtDetails]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
        return result.List;
    }
    public async Task<CheckCustomerAccountExistsDto> CheckCustomerAccountExistsAsync(CheckCustomerAccountExistsDm dataModel, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dataModel.AccountNumber))
        {
            _logger.Warn("AccountNumber is null or empty in CheckCustomerAccountExistsAsync.");
            return new CheckCustomerAccountExistsDto { Exists = false };
        }

        var parameters = new List<SqlParameter>
        {
            new("@accountNumber", SqlDbType.VarChar, 50) { Value = dataModel.AccountNumber }
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[CheckCustomerAccountExists]",
            Parameters = parameters
        }, cancellationToken);

        var exists = result.Result != null && Convert.ToBoolean(result.Result);

        return new CheckCustomerAccountExistsDto { Exists = exists };
    }

    public async Task<GetMosavabeNoByCaseIdDto> GetMosavabeNoByCaseId(GetMosavabeNoByCaseIdDataModel dataModel, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@CaseId", SqlDbType.NVarChar) {Value = dataModel.CaseId},
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[GetMosavabeNoByCaseId]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new GetMosavabeNoByCaseIdDto()
        {
            MosavabeNumber = result.Result.ToString()
        };
    }



    public async Task<PaymentPermissionIssuanceControlForLcpDto> PaymentPermissionIssuanceControlForLcp(PaymentPermissionIssuanceControlForLcpDM dataModel, CancellationToken cancellationToken = default)
    {
        var parameters = new List<SqlParameter>()
        {
            new("@MosavabeNumber", SqlDbType.NVarChar) {Value = dataModel.MosavabeNumber},
            new("@CustomerNumber", SqlDbType.NVarChar) {Value = dataModel.CustomerNumber}
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[PaymentPermissionIssuanceControlForLcp]",

            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        return new PaymentPermissionIssuanceControlForLcpDto()
        {
            Result = result.Result!.ToString()
        };
    }
}