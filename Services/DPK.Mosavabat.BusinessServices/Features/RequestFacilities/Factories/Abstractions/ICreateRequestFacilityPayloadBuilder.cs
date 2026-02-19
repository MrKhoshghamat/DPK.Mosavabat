using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.Bizagi;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Factories.Abstractions;

public interface ICreateRequestFacilityPayloadBuilder
{
    CreateRequestFacilityBizagiDm Build(
        CreateRequestFacilityCommand command,
        string guid);
}