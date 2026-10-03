using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts.Submission;

public interface ILcpRequestFacilitySubmissionService
{
    Task<MosavabatResponseDataModel?> SubmitAsync(
        RequestFacilityUpdateDataModel requestFacility, CancellationToken ct = default);
}