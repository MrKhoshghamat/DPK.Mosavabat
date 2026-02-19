using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateRequestFacilityStatusesCommand(
    List<RequestFacilityStatusCommand> requestFacilityStatuses)
{
    public List<RequestFacilityStatusCommand> RequestFacilityStatuses { get; } = requestFacilityStatuses;
}