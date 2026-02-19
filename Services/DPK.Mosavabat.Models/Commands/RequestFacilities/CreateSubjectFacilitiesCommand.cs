using System.Collections.Generic;

namespace DPK.Mosavabat.Models.Commands.RequestFacilities;

public class CreateSubjectFacilitiesCommand(
    List<SubjectFacilityCommand> subjectFacilities)
{
    public List<SubjectFacilityCommand> SubjectFacilities { get; } = subjectFacilities;
}