using System.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog.Context;
using Serilog;

namespace DPK.Mosavabat.GeneralServices.Logging;

public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var layerName = requestName
            .EndsWith("Query")
            ? "QueryHandler"
            : "CommandHandler";

        using (LogContext.PushProperty("Layer", layerName))
        using (LogContext.PushProperty("RequestName", requestName))
        {
            Log.Information("Handling {RequestName}...", requestName);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next(cancellationToken);
                stopwatch.Stop();

                Log.Information("Handled {RequestName} successfully in {Elapsed}ms", requestName, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Handler Failed for {RequestName} in {Elapsed}ms", requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}