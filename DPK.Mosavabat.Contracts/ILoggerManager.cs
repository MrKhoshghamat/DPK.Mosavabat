namespace DPK.Mosavabat.Contracts;

public interface ILoggerManager
{
    void Info(string? message);
    void Warn(string message);
    void Debug(string message);
    void Error(string message);
    void Trace(string message);
    void Fatal(string message);
}