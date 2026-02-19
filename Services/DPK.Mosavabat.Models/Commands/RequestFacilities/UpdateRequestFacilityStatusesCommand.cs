using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateRequestFacilityStatusesCommand(
    List<RequestFacilityStatusCommand> requestFacilityStatusCommands)
{
    public List<RequestFacilityStatusCommand> RequestFacilityStatusCommands { get; } = requestFacilityStatusCommands;
}