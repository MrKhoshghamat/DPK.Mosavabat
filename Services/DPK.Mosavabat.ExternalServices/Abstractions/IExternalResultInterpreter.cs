namespace DPK.Mosavabat.ExternalServices.Abstractions;

public interface IExternalResultInterpreter
{
    T EnsureSuccess<T>(ExternalCallResult<T> result);
}