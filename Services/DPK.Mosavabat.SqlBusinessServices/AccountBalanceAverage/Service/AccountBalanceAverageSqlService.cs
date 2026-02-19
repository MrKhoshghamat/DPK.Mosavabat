using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DPK.Mosavabat.Models.DataModels.AccountBalanceAverage;
using DPK.Mosavabat.Models.EntitiesDto.AccountBalanceAverage;
using DPK.Mosavabat.SqlBusinessServices.AccountBalanceAverage.Contract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace DPK.Mosavabat.SqlBusinessServices.AccountBalanceAverage.Service;

public class AccountBalanceAverageSqlService(
    IConfiguration configuration,
    ILogger<AccountBalanceAverageSqlService> logger)
    : IAccountBalanceAverageSqlService
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

    public async Task<List<AccountBalanceAverageDto>> GetAccountBalanceAverage(
        GetAccountBalanceAverageDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[aba].[GetAccountBalanceAverage]";
        var parameters = new
        {
            customerNo = dm.CustomerNo,
            approvalRequestId = dm.ApprovalRequestId,
            cuMosavabeId = dm.CuMosavabeId,
            typeOfContractLength = dm.TypeOfContractLength,
            maxContractLength = dm.MaxContractLength,
            code = dm.Code,
            currentMonth = dm.CurrentMonth,
            facilityGroup = dm.FacilityGroup
        };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<AccountBalanceAverageDto>(command);
            return result.ToList();
        }, cancellationToken);
    }

    public async Task<List<AccountBalanceAverageDto>> GetRelatedAccountBalanceAverage(
        GetRelatedAccountBalanceAverageDm dm,
        CancellationToken cancellationToken = default)
    {
        const string commandText = "[aba].[GetRelatedAccountBalanceAverage]";
        var parameters = new
        {
            customerNo = dm.CustomerNo,
            accountNumber = dm.AccountNo,
            approvalRequestId = dm.ApprovalRequestId,
            cuMosavabeId = dm.CuMosavabeId,
            typeOfContractLength = dm.TypeOfContractLength,
            maxContractLength = dm.MaxContractLength,
            code = dm.Code,
            currentMonth = dm.CurrentMonth,
            facilityGroup = dm.FacilityGroup
        };

        return await ExecuteWithLoggingAsync(commandText, parameters, async (connection) =>
        {
            var command = new CommandDefinition(
                commandText: commandText,
                parameters: parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            var result = await connection.QueryAsync<AccountBalanceAverageDto>(command);
            return result.ToList();
        }, cancellationToken);
    }
}