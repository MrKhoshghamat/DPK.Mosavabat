namespace DPK.Mosavabat.Common.Idempotency;

public enum WorkflowCallStatus
{
    Succeeded,
    ExplicitFailure,
    TransportFailure,
    InvalidResponse
}