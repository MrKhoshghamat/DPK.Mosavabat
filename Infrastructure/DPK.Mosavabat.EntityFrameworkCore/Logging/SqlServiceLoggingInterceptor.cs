using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog.Context;
using Serilog;

namespace DPK.Mosavabat.EntityFrameworkCore.Logging;

public class SqlServiceLoggingInterceptor : DbCommandInterceptor
{
    public override async Task CommandFailedAsync(
        DbCommand command,
        CommandErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        LogSqlError(
            command,
            eventData.Exception);

        await base.CommandFailedAsync(
            command,
            eventData,
            cancellationToken);
    }

    public override void CommandFailed(
        DbCommand command,
        CommandErrorEventData eventData)
    {
        LogSqlError(
            command,
            eventData.Exception);

        base.CommandFailed(
            command,
            eventData);
    }

    private void LogSqlError(
        DbCommand command,
        Exception ex)
    {
        using (LogContext.PushProperty("Layer", "SqlService"))
        {
            Log.Error(ex, "SQL Command Failed. Command Text: {CommandText}", command.CommandText);
        }
    }
}