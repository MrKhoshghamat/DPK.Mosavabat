using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Common.Primitives; // فرض بر وجود کلاس‌های Result و Error
using DPK.Mosavabat.Models.DataModels.ApprovalRequests;
using DPK.Mosavabat.Models.DataModels.RequestFacilities;
using DPK.Mosavabat.Models.EntitiesDto.ApprovalRequests;
using DPK.Mosavabat.Models.EntitiesDto.RequestFacilities;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Service;

public class RequestFacilitySqlService(
    IConfiguration configuration,
    ILogger<RequestFacilitySqlService> logger)
    : IRequestFacilitySqlService
{
    private readonly string? _connectionString = configuration
        .GetConnectionString("MosavabatConnection");

    // Wrapper برای مدیریت کانکشن، لاگینگ و هندلینگ خطاها
    private async Task<T> ExecuteWithLoggingAsync<T>(
        string commandText,
        object parameters,
        Func<SqlConnection, Task<T>> databaseAction,
        CancellationToken cancellationToken,
        [CallerMemberName] string operationName = "")
    {
        using (LogContext.PushProperty("Layer", "SqlService"))
        using (LogContext.PushProperty("DapperCommand", commandText))
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                var result = await databaseAction(connection);

                sw.Stop();

                if (sw.ElapsedMilliseconds > 500)
                {
                    logger.LogWarning(
                        "Long running SQL {OperationName} took {ElapsedMilliseconds} ms. Parameters: {@Parameters}",
                        operationName, sw.ElapsedMilliseconds, parameters);
                }

                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                logger.LogError(ex,
                    "SQL Error in {OperationName} after {ElapsedMilliseconds} ms. Command: {CommandText}, Parameters: {@Parameters}",
                    operationName, sw.ElapsedMilliseconds, commandText, parameters);
                throw;
            }
        }
    }

    public async Task<Result<UpdateLcpRequestFacilityDto>> UpdateLcpRequestFacility(
        UpdateLcpRequestFacilityDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[UpdateLcpRequestFacility]";
        var parameters = new
        {
            idCase = dm.IdCase,
            initialRadNumber = dm.InitialRadNumber,
            idCaseApprovalRequest = dm.IdCaseApprovalRequest,
            radNumberApprovalRequest = dm.RadNumberApprovalRequest,
            lcpRequestFacilityId = dm.LcpRequestFacilityId,
            guidLcpRequestFacility = dm.GuidLcpRequestFacility
        };

        var rowsAffected = await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            return await connection.ExecuteAsync(command);
        }, cancellationToken);

        // اگر آپدیت برای بیزنس حیاتی است و انجام نشدنش خطاست، می‌توان اینجا Failure برگرداند
        // اما فعلاً طبق DTO فقط وضعیت را برمی‌گردانیم
        return new UpdateLcpRequestFacilityDto(rowsAffected > 0);
    }

    public async Task<Result<MapLcpRequestFacilityToApprovalRequestDto>> MapLcpRequestFacilityToApprovalRequest(
        MapLcpRequestFacilityToApprovalRequestDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[LCP].[MapLcpRequestFacilityToApprovalRequest]";
        var parameters = new
        {
            idLCP_RequestFacility = dm.IdLcpRequestFacility,
            idCase = dm.IdCase,
            idCaseApprovalRequest = dm.IdCaseApprovalRequest
        };

        var rowsAffected = await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            return await connection.ExecuteAsync(command);
        }, cancellationToken);

        return new MapLcpRequestFacilityToApprovalRequestDto(rowsAffected > 0);
    }

    public async Task<Result<GetUsernameByBranchCodeDto>> GetUsernameByBranchCode(
        GetUsernameByBranchCodeDm dm,
        CancellationToken cancellationToken = default)
    {
        var commandText = dm.MosavabatClassName == MosavabatClassName.AgentRegistry
            ? "[LCP].[FindBranchManagerOrSupervisor]"
            : "[LCP].[FindFacilityManagerOrSupervisor]";

        var parameters = new { branchCode = dm.BranchCode };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetUsernameByBranchCodeDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetUsernameByBranchCodeDto>(
                new Error("Sql.UserNotFound", $"کاربری برای کد شعبه {dm.BranchCode} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetCollateralTypeIdDto>> GetCollateralTypeId(
        GetCollateralTypeIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[GetCollateralTypeId]";
        var parameters = new { Code = dm.CollateralTypeCode };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetCollateralTypeIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetCollateralTypeIdDto>(
                new Error("Sql.CollateralTypeNotFound", $"نوع وثیقه با کد {dm.CollateralTypeCode} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<ExtractExchangeCommercialDocIdDto>> ExtractExchangeCommercialDocId(
        ExtractExchangeCommercialDocIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[ExtractExchangeCommercialDocId]";
        var parameters = new { dm.DocumentCode };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<ExtractExchangeCommercialDocIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<ExtractExchangeCommercialDocIdDto>(
                new Error("Sql.DocumentNotFound", $"سند تجاری با کد {dm.DocumentCode} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<ExtractSubContractTypeIdDto>> ExtractSubContractTypeId(
        ExtractSubContractTypeIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[ExtractSubContractTypeId]";
        var parameters = new { dm.ContractTemplateCode };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<ExtractSubContractTypeIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<ExtractSubContractTypeIdDto>(
                new Error("Sql.ContractTypeNotFound", $"نوع قرارداد با کد {dm.ContractTemplateCode} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<ExtractFacilityGroupIdDto>> ExtractFacilityGroupId(
        ExtractFacilityGroupIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[ExtractFacilityGroupId]";
        var parameters = new { Code = dm.FacilityGroupCode };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<ExtractFacilityGroupIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<ExtractFacilityGroupIdDto>(
                new Error("Sql.FacilityGroupNotFound", $"گروه تسهیلات با کد {dm.FacilityGroupCode} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetApprovalRequestIdDto>> GetApprovalRequestId(
        GetApprovalRequestIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[GetApprovalRequestId]";
        var parameters = new { dm.CaseId };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetApprovalRequestIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetApprovalRequestIdDto>(
                new Error("Sql.ApprovalRequestNotFound", $"درخواست مصوبه‌ای برای کیس {dm.CaseId} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<UpdateRequestFacilityStatusDto>> UpdateRequestFacilityStatus(
        UpdateRequestFacilityStatusDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[UpdateReqFacilityStatus]";
        var parameters = new { dm.CaseId };

        var rowsAffected = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteAsync(command);
            }, cancellationToken);

        return new UpdateRequestFacilityStatusDto(rowsAffected > 0);
    }

    public async Task<Result<UpdateRequestFacilityRadNumberDto>> UpdateRequestFacilityRadNumber(
        UpdateRequestFacilityRadNumberDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[LCP].[UpdateRequestFacilityRadNumber]";
        var parameters = new
        {
            idApprovalRequest = dm.IdApprovalRequest,
            radNumber = dm.RadNumber
        };

        var rowsAffected = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteAsync(command);
            }, cancellationToken);

        return new UpdateRequestFacilityRadNumberDto(rowsAffected > 0);
    }

    public async Task<Result<GetUserNameByCaseIdDto>> GetUserNameByCaseId(
        GetUserNameByCaseIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetUserNameByCaseId]";
        var parameters = new { caseId = dm.CaseId };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetUserNameByCaseIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetUserNameByCaseIdDto>(
                new Error("Sql.UserNotFound", $"کاربری برای کیس {dm.CaseId} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetUserIdByUsernameDto>> GetUserIdByUsername(
        GetUserIdByUsernameDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetUserIdByUsername]";
        var parameters = new { username = dm.Username };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetUserIdByUsernameDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetUserIdByUsernameDto>(
                new Error("Sql.UserNotFound", $"کاربری با نام کاربری {dm.Username} یافت نشد."));
        }

        return result;
    }

    public async Task<Result<GetRadNumberByCaseIdDto>> GetRadNumberByCaseId(
        GetRadNumberByCaseIdDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[dbo].[GetRadNumberByCaseId]";
        var parameters = new { caseId = dm.CaseId };

        var result = await ExecuteWithLoggingAsync(
            commandText,
            parameters,
            async (connection) =>
            {
                var command = new CommandDefinition(
                    commandText: commandText,
                    parameters: parameters,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                );

                return await connection.ExecuteScalarAsync<GetRadNumberByCaseIdDto>(command);
            }, cancellationToken);

        if (result is null)
        {
            return Result.Failure<GetRadNumberByCaseIdDto>(
                new Error("Sql.RadNumberNotFound", $"شماره پرونده (RadNumber) برای کیس {dm.CaseId} یافت نشد."));
        }

        return result;
    }
}
