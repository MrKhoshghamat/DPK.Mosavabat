using Serilog.Context;
using Serilog;
using System.Diagnostics;

namespace DPK.Mosavabat.ExternalServices.Logging;

public class ExternalServiceLoggingHandler
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var serviceName = request.RequestUri?.Host ?? "ExternalService";

        using (LogContext.PushProperty("Layer", "ExternalService"))
        using (LogContext.PushProperty("ExternalSystem", serviceName))
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await base.SendAsync(
                    request,
                    cancellationToken);

                stopwatch.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("External API Error. Url: {Url}, StatusCode: {StatusCode}, Time: {Elapsed}ms",
                        request.RequestUri, response.StatusCode, stopwatch.ElapsedMilliseconds);
                }

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error(ex, "External API Exception. Url: {Url}, Time: {Elapsed}ms",
                    request.RequestUri, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}