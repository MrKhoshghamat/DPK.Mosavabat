using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat.Contract;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IApprovalRequestDatabase
{
    Task<List<RequestFacilityStatusLevelDataModel>?> GetRequestStatus(GetRequestStatuesDataModel getRequestStatuesDataModel);
}