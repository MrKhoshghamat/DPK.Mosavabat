// ReSharper disable once IdentifierTypo

namespace DPK.Mosavabat.Entities.Exceptions;

public sealed class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() : base("Parameter ids is null")
    {
    }
}