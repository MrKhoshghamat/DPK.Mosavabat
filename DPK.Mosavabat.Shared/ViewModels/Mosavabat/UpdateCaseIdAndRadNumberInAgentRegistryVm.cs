using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;

namespace DPK.Mosavabat.Shared.ViewModels.Mosavabat;

public class UpdateCaseIdAndRadNumberInAgentRegistryVm
{
    public AgentDataModel? Agent { get; set; }
    public int CaseId { get; set; }
    public int RadNumber { get; set; }
}
