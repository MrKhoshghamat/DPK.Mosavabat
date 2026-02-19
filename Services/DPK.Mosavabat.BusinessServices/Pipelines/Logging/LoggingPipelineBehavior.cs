using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.Common.Primitives;
using MediatR;
using Serilog;
using Serilog.Context;

namespace DPK.Mosavabat.BusinessServices.Pipelines.Logging;

/// <summary>
/// پایپلاین لاگ گیری در سطح هندلر
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// هندلر
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var shortName = requestName.Replace("Command", "").Replace("Query", "");

        using (LogContext.PushProperty("Layer", "BusinessHandler"))
        using (LogContext.PushProperty("RequestName", requestName))
        {
            Log.Information("Handling {ShortName}...", shortName);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await next(cancellationToken);
                stopwatch.Stop();

                if (response is Result { IsFailure: true } resultBase)
                {
                    Log.Warning("Handled {ShortName} with Business Error in {Elapsed}ms. Error: {ErrorMessage}",
                        shortName,
                        stopwatch.ElapsedMilliseconds,
                        resultBase.Error.Message);

                    return response;
                }

                Log.Information("Handled {ShortName} successfully in {Elapsed}ms", shortName, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "Handler Failed for {ShortName} in {Elapsed}ms", shortName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
