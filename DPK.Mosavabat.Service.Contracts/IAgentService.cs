using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.Services.Contracts;

public interface IAgentService
{
    Task<MosavabatResponseDataModel?> RegisterAgentAsync(MosavabatAgentDataModel agent);
}