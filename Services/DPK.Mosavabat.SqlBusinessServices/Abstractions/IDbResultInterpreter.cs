namespace DPK.Mosavabat.SqlBusinessServices.Abstractions;

public interface IDbResultInterpreter
{
    void EnsureSuccess<T>(DbExecutionResult<T> result);
}