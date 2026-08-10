using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts;

public interface IApprovalRequestService
{
    Task<List<RequestFacilityStatusLevelDataModel>?> GetRequestStatus(GetRequestStatuesDataModel getRequestStatuesDataModel);
    Task<MosavabatResponseDataModel?> CreateApprovalRequest(CreateApprovalRequestDataModel createApprovalRequestDataModel);
}