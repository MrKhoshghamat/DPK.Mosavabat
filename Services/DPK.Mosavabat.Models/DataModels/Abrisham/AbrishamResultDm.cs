namespace DPK.Mosavabat.Models.DataModels.Abrisham;

public class AbrishamResultDm(
    int code,
    string karizMessage,
    string message,
    string referenceId,
    string timestamp)
{
    public int Code { get; } = code;
    public string KarizMessage { get; } = karizMessage;
    public string Message { get; } = message;
    public string ReferenceId { get; } = referenceId;
    public string Timestamp { get; } = timestamp;
}