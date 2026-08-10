using DPK.Mosavabat.Shared.DataModels.Lcp;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;

namespace DPK.Mosavabat.Services.Contracts;

public interface ILcpService
{
    Task<string> AcceptAgent(AcceptAgentDataModel dataModel);
}