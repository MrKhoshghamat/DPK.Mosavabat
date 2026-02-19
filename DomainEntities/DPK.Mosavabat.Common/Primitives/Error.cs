namespace DPK.Mosavabat.Common.Primitives;

public record Error(
    string Code,
    string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "مقدار وارد شده نال است.");

    public static implicit operator Result(Error error) => Result.Failure(error);
}