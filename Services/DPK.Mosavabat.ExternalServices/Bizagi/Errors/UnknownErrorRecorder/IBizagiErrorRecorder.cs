using DPK.Foundation.Dependency;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Errors.UnknownErrorRecorder;

public interface IBizagiErrorRecorder
    : ISingletonDependency
{
    void RecordUnknownError(
        string errorCode);
}