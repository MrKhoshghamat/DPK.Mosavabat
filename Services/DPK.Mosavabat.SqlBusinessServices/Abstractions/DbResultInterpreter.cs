using DPK.Mosavabat.Common.Exceptions.Business;
using System.Collections.Generic;

namespace DPK.Mosavabat.SqlBusinessServices.Abstractions;

public sealed class DbResultInterpreter(
    IBusinessErrorCatalog catalog)
    : IDbResultInterpreter
{
    public void EnsureSuccess<T>(
        DbExecutionResult<T> result)
    {
        if (result is null)
            throw BusinessRuleException.FromSingle(
                new BusinessError(
                    -1,
                    "نتیجه دیتابیس دریافت نشد."));

        if (result.IsSuccess)
            return;

        if (catalog.TryGet(ParseErrorCode(result.RawResult), out var error))
        {
            throw BusinessRuleException.FromSingle(error, new Dictionary<string, object?>
            {
                ["RawResult"] = result.RawResult
            });
        }

        throw BusinessRuleException.FromSingle(
            new BusinessError(-1, "خطای ناشناخته دیتابیس"),
            new Dictionary<string, object?>
            {
                ["RawResult"] = result.RawResult
            });
    }

    private static int ParseErrorCode(
        string? rawResult)
        => int.TryParse(
            rawResult,
            out var code)
            ? code : -1;
}