using DPK.Mosavabat.Shared.DataModels.Lcp;

namespace DPK.Mosavabat.External.Services.Contracts.Lcp;

public interface IAgentServices
{
    Task<string?> AcceptAgent(AcceptAgentDataModel acceptAgent);
}