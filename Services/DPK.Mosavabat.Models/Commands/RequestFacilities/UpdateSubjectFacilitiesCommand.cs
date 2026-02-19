using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class UpdateSubjectFacilitiesCommand(
    List<SubjectFacilityCommand> subjectFacilityCommands)
{
    public List<SubjectFacilityCommand> SubjectFacilityCommands { get; } = subjectFacilityCommands;
}