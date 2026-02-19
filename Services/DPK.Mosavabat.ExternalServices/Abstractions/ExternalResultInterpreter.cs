using DPK.Mosavabat.Common.Exceptions.Business;

namespace DPK.Mosavabat.ExternalServices.Abstractions;

public sealed class ExternalResultInterpreter(IBusinessErrorCatalog catalog) : IExternalResultInterpreter
{
    public T EnsureSuccess<T>(
        ExternalCallResult<T> result)
    {
        if (result is { IsSuccess: true, Payload: { } payload })
            return payload;

        if (result.Error is null)
            throw BusinessRuleException.FromSingle(
                new BusinessError(
                    -1,
                    "خطای نامشخص سرویس بیرونی"));

        if (catalog.TryGet(MapExternalCode(result.Error), out var businessError))
        {
            throw BusinessRuleException.FromSingle(
                businessError,
                new Dictionary<string, object?>
                {
                    ["Provider"] = result.Error.Provider,
                    ["ExternalCode"] = result.Error.Code,
                    ["RawPayload"] = result.Error.RawPayload
                });
        }

        throw BusinessRuleException.FromSingle(
            new BusinessError(-1, result.Error.Message),
            new Dictionary<string, object?>
            {
                ["Provider"] = result.Error.Provider,
                ["ExternalCode"] = result.Error.Code,
                ["RawPayload"] = result.Error.RawPayload
            });
    }

    private static int MapExternalCode(ExternalError error)
        => int.TryParse(error.Code, out var code) ? code : -1;
}