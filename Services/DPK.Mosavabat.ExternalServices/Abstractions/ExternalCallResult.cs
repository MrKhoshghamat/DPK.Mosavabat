namespace DPK.Mosavabat.ExternalServices.Abstractions;

public sealed class ExternalCallResult<T>
{
    private ExternalCallResult(
        bool isSuccess,
        T? payload,
        ExternalError? error)
        => (IsSuccess, Payload, Error) = (isSuccess, payload, error);

    public bool IsSuccess { get; }
    public T? Payload { get; }
    public ExternalError? Error { get; }

    public static ExternalCallResult<T> Success(T payload)
        => new(true, payload, null);

    public static ExternalCallResult<T> Failure(ExternalError error)
        => new(false, default, error);
}