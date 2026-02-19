namespace DPK.Mosavabat.SqlBusinessServices.Abstractions;

public sealed class DbExecutionResult<T>
{
    private DbExecutionResult(
        bool isSuccess,
        T? payload,
        string? rawResult) => (IsSuccess, Payload, RawResult) = (isSuccess, payload, rawResult);

    public bool IsSuccess { get; }
    public T? Payload { get; }
    public string? RawResult { get; }

    public static DbExecutionResult<T> Success(
        T data,
        string? rawResult = null)
        => new(true, data, rawResult);

    public static DbExecutionResult<T> Failure(
        string rawResult)
        => new(false, default, rawResult);
}