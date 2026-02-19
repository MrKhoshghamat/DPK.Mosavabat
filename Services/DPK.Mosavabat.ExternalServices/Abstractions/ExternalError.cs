namespace DPK.Mosavabat.ExternalServices.Abstractions;

public sealed record ExternalError(
    string Provider,
    string Code,
    string Message,
    object? RawPayload = null);