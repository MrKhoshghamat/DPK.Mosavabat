using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Lcp;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Database;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace DPK.Mosavabat.Database.Services.Mosavabat;

public class AgentDatabaseService : MainDatabaseService, IAgentDatabaseService
{
    private readonly ILoggerManager _logger;

    public AgentDatabaseService(
        IConfiguration configuration,
        ILoggerManager logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<UpdateCaseIdAndRadNumberInAgentRegistryDto> UpdateCaseIdAndRadNumberInAgentRegistry(
    UpdateCaseIdAndRadNumberInAgentRegistryDm dataModel,
    CancellationToken cancellationToken)
    {
        if (dataModel.Agent == null)
        {
            _logger.Warn("Input dataModel or Agent properties are null.");
            return new UpdateCaseIdAndRadNumberInAgentRegistryDto { IsSucceed = false };
        }

        var parameters = new List<SqlParameter>
    {
        new("@branchCode", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.BranchCode! },
        new("@agentNationalCode", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.NationalCode ! },
        new("@legalNationalId", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.LegalNationalId ! },
        new("@caseId", SqlDbType.Int) { Value = dataModel.CaseId },
        new("@radNumber", SqlDbType.Int) { Value = dataModel.RadNumber }
    };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[UpdateCaseIdAndRadNumberInAgentRegistry]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        var isSucceed = false;
        if (result.Result != null)
        {
            isSucceed = Convert.ToBoolean(result.Result);
        }

        return new UpdateCaseIdAndRadNumberInAgentRegistryDto { IsSucceed = isSucceed };
    }

    public async Task<CheckAgentRegistryExistsDto> CheckAgentRegistryExists(
        CheckAgentRegistryExistsDm dataModel,
        CancellationToken cancellationToken)
    {
        if (dataModel?.Agent == null)
        {
            _logger.Warn("Input dataModel or Agent properties are null in CheckAgentRegistryExists.");
            return new CheckAgentRegistryExistsDto { Exists = false };
        }

        var parameters = new List<SqlParameter>
        {
            new("@branchCode", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.BranchCode! },
            new("@agentNationalCode", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.NationalCode ! },
            new("@legalNationalId", SqlDbType.NVarChar, 50) { Value = dataModel.Agent.LegalNationalId ! }
        };

        var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "[LCP].[CheckAgentRegistryExists]",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));

        var exists = false;
        if (result.Result != null)
        {
            exists = Convert.ToBoolean(result.Result);
        }

        return new CheckAgentRegistryExistsDto { Exists = exists };
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

    public async Task LcpAgentAbortCase(int caseId, CancellationToken cancellationToken)
    {
        var parameters = new List<SqlParameter>
        {
            new("@caseId", SqlDbType.Int) {Value = caseId}
        };

        var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
        {
            ConnectionString = DevProviderConnectionString,
            StoredProcedureName = "dbo.LCP_AgentAbortCase",
            Parameters = parameters
        }, cancellationToken);

        _logger.Trace(JsonConvert.SerializeObject(result));
    }
}
