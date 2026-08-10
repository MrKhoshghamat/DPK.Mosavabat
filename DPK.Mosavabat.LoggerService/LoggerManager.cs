using DPK.Mosavabat.Contracts;
using NLog;

namespace DPK.Mosavabat.LoggerService;

public class LoggerManager : ILoggerManager
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    // Trace (Ordinal = 0): Most verbose level.Used for development and seldom enabled in production.
    public void Trace(string message) => Logger.Trace(message);

    // Debug (Ordinal = 1): Debugging the application behavior from internal events of interest.
    public void Debug(string message) => Logger.Debug(message);

    // Info (Ordinal = 2): Information that highlights progress or application lifetime events.
    public void Info(string? message) => Logger.Info(message);

    // Warn (Ordinal = 3): Warnings about validation issues or temporary failures that can be recovered.
    public void Warn(string message) => Logger.Warn(message);

    // Error (Ordinal = 4): Errors where functionality has failed or Exception have been caught.
    public void Error(string message) => Logger.Error(message);

    // Fatal (Ordinal = 5): Most critical level.Application is about to abort.
    public void Fatal(string message) => Logger.Fatal(message);
}