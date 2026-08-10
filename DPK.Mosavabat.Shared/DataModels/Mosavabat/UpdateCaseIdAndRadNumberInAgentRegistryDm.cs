using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.Shared.DataModels.Mosavabat;

public class UpdateCaseIdAndRadNumberInAgentRegistryDm
{
    public AgentDataModel? Agent { get; set; }
    public int CaseId { get; set; }
    public int RadNumber { get; set; }
}
