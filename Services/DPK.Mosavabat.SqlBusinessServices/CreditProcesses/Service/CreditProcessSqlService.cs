using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DPK.Mosavabat.Common.Consts.General;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Service;

public class CreditProcessSqlService(
    IConfiguration configuration,
    ILogger<CreditProcessSqlService> logger)
    : ICreditProcessSqlService
{
    private readonly string? _connectionString = configuration
        .GetConnectionString("MosavabatConnection");
    
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

    public async Task<List<MosavabeContractsDto>> GetMosavabeContracts(
        GetMosavabeContractsDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetMosavabeContracts]";
        var parameters = new { mosavabeNo = dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<MosavabeContractsDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<List<CustomerCurrentFacilityDto>> GetCustomerCurrentFacilities(
        GetCustomerCurrentFacilitiesDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetCurrentFacilities]";
        var parameters = new { customerNo = dm.CustomerNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<CustomerCurrentFacilityDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<GetCustomerNoByMosavabeNoDto> GetCustomerNoByMosavabeNo(
        GetCustomerNoByMosavabeNoDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetMosavabeContracts]";
        var parameters = new { mosavabeNo = dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetCustomerNoByMosavabeNoDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<CalculateApprovalRequestUnusedBalanceDto> CalculateApprovalRequestUnusedBalance(
        CalculateApprovalRequestUnusedBalanceDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[CalculationCustomerDebtBalance]";
        var parameters = new { mosavabeNumber = dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<CalculateApprovalRequestUnusedBalanceDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<CalculateDailyPaidContractsAmountsSumDto> CalculateDailyPaidContractsAmountsSum(
        CalculateDailyPaidContractsAmountsSumDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[CalculateDailyPaidContractsAmountsSum]";
        var parameters = new { CustNo = dm.CustomerNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<CalculateDailyPaidContractsAmountsSumDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<ControlPaymentPermissionIssuanceDto> ControlPaymentPermissionIssuance(
        ControlPaymentPermissionIssuanceDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[PaymentPermissionIssuanceControl]";
        var parameters = new
        {
            dm.MosavabeNo,
            dm.ContractAmount,
            dm.CustomerSharePrice,
            dm.SubContractType,
            dm.CustomerNo
        };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<ControlPaymentPermissionIssuanceDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<UpdateIsDeletedCuContractDto> UpdateIsDeletedCuContract(
        UpdateIsDeletedCuContractDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[dbo].[UpdateIsDeletedCuContract]";
        var parameters = new
        {
            dm.PaymentPermissionId,
            dm.IsDeleted,
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

        return new UpdateIsDeletedCuContractDto(rowsAffected > 0);
    }

    public async Task<GetBranchCodeByC13Dto> GetBranchCodeByC13(
        GetBranchCodeByC13Dm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetBranchCodeByC13]";
        var parameters = new { dm.C13 };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetBranchCodeByC13Dto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<FindBranchManagerOrSupervisorDto> FindBranchManagerOrSupervisor(
        FindBranchManagerOrSupervisorDm dm,
        CancellationToken cancellationToken)
    {
        var commandText = dm.MosavabatClassName == MosavabatClassName.AgentRegistry
            ? "[LCP].[FindBranchManagerOrSupervisor]"
            : "[LCP].[FindFacilityManagerOrSupervisor]";

        var parameters = new { dm.BranchCode };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<FindBranchManagerOrSupervisorDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<GetAccNoByC13Dto> GetAccNoByC13(
        GetAccNoByC13Dm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetAccNoByC13]";
        var parameters = new { dm.C13 };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetAccNoByC13Dto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<GetDateGByC13Dto> GetDateGByC13(
        GetDateGByC13Dm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetDateGByC13]";
        var parameters = new { dm.C13 };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetDateGByC13Dto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<GetC16ByC13Dto> GetC16ByC13(
        GetC16ByC13Dm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetC16ByC13]";
        var parameters = new { dm.C13 };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetC16ByC13Dto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<GetCustomerEkDto> GetCustomerEk(
        GetCustomerEkDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[dbo].[GetCustomerEk]";
        var parameters = new { AccountNo = dm.AccNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetCustomerEkDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<UpdateCuCustomerInCuContractDto> UpdateCuCustomerInCuContract(
        UpdateCuCustomerInCuContractDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[dbo].[UpdateCuCustomerInCuContract]";
        var parameters = new
        {
            IdCu_Customer = dm.IdCuCustomer,
            dm.C13,
            dm.C16
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

        return new UpdateCuCustomerInCuContractDto(rowsAffected > 0);
    }

    public async Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(
        GetApprovalRequestInfoDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetApprovalRequestInfoByMosavabeNo]";
        var parameters = new { dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryFirstOrDefaultAsync<GetApprovalRequestInfoDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<List<ApprovalRequestTermsAndConditionsDto>> GetApprovalRequestTermsAndConditions(
        GetApprovalRequestTermsAndConditionsDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetListTermsAndConditionsApprovalRequest]";
        var parameters = new { MosavabeNumber = dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<ApprovalRequestTermsAndConditionsDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<List<CuContractDto>> GetCuContracts(
        GetCuContractsDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetCuContracts]";
        var parameters = new { MosavabeNumber = dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<CuContractDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<List<CustomerPastDebtDto>> GetCustomerContractsPastDebts(
        GetCustomerContractsPastDebtsDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetCustomerContractsPastDebt]";
        var parameters = new { CustNo = dm.CustomerNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<CustomerPastDebtDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<List<CustomerDailyPaidContractDto>> GetCustomerDailyPaidContracts(
        GetCustomerDailyPaidContractsDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetDailyPaidContracts]";
        var parameters = new { CustNo = dm.CustomerNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<CustomerDailyPaidContractDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(
        GetLiveApprovalRequestsAmountsSumDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[dbo].[CalculateLiveApprovalRequestsAmountsSum]";
        var parameters = new
        {
            CustNo = dm.CustomerNo,
            dm.IdApprovalRequest
        };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetLiveApprovalRequestsAmountsSumDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<GetSubjectTypeByMosavabeNoDto> GetSubjectTypeByMosavabeNo(
        GetSubjectTypeByMosavabeNoDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[GetSubjectTypeByMosavabeNo]";
        var parameters = new { dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<GetSubjectTypeByMosavabeNoDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(
        IsAllowedRecalculateAccountBalanceAverageDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[IsAllowedRecalculateAccountBalanceAverage]";
        var parameters = new
        {
            mosavabeNo = dm.MosavabeNo,
            contractAmount = dm.ContractAmount
        };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<IsAllowedRecalculateAccountBalanceAverageDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<IsExistActivePaymentPermissionDto> IsExistActivePaymentPermission(
        IsExistActivePaymentPermissionDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[CheckExistActivePaymentPermission]";
        var parameters = new { dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<IsExistActivePaymentPermissionDto>(command);
            return result!;
        }, cancellationToken);
    }

    public async Task<IsLcpRequestDto> IsLcpRequest(
        IsLcpRequestDm dm,
        CancellationToken cancellationToken)
    {
        const string commandText = "[CP].[IsLcpRequest]";
        var parameters = new { dm.MosavabeNo };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.ExecuteScalarAsync<IsLcpRequestDto>(command);
            return result!;
        }, cancellationToken);
    }
}
