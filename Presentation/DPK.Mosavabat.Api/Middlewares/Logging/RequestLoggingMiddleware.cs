using System.Diagnostics;
using Serilog;
using Serilog.Context;

namespace DPK.Mosavabat.Api.Middlewares.Logging;

/// <summary>
/// میدلور ایجاد لاگ در سطح api
/// </summary>
/// <remarks>
/// سازنده
/// </remarks>
/// <param name="next"></param>
public class RequestLoggingMiddleware(
    RequestDelegate next)
{

    /// <summary>
    /// اجرا
    /// </summary>
    /// <param name="context"></param>
    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"]
            .FirstOrDefault() ?? Guid.NewGuid().ToString();

        var username = context.User.Identity?.Name ?? "Anonymous";

        var ip = context.Connection.RemoteIpAddress?.ToString();

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("UserName", username))
        using (LogContext.PushProperty("ClientIP", ip))
        using (LogContext.PushProperty("Layer", "Api"))
        {
            var stopwatch = Stopwatch.StartNew();

            await next(context);

            stopwatch.Stop();

            Log.Information(
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}