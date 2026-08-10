namespace DPK.Mosavabat.Shared.Exceptions.ExternalServices;

public class SamatExternalServiceUnavailableException : Exception
{
    public SamatExternalServiceUnavailableException(string message) : base(message) { }
    public SamatExternalServiceUnavailableException(string message, Exception innerException)
        : base(message, innerException) { }
}