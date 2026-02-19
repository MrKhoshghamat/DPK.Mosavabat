using DPK.Foundation.Dependency;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Errors.MessageProvider;

public interface IBizagiMessageProvider
    : ISingletonDependency
{
    string GetMessage(
        string? errorCode);
}