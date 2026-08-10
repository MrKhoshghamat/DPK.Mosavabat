using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface IRequestFacilityServices
{
    Task<string?> AcceptRequestFacility(UpdateRequestFacilityDataModel updateRequestFacilityDataModel);
    Task<string?> UpdateRadNumberRequestFacility(UpdateRequestFacilityRadNumber updateRequestFacilityRadNumber);
}