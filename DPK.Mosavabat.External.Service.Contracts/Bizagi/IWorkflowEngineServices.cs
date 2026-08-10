using System.Xml;
using DPK.Mosavabat.Shared.Templates.Bizagi;

namespace DPK.Mosavabat.External.Services.Contracts.Bizagi;

public interface IWorkflowEngineServices
{
    Task<MosavabatResponseDataModel?> CreateCasesAsync(XmlNode casesInfo);
    Task<MosavabatResponseDataModel?> PerformActivityAsync(XmlNode casesInfo);
    Task<MosavabatResponseDataModel?> SaveActivityAsync(XmlNode casesInfo);
    Task<MosavabatResponseDataModel?> AbortCasesAsync(XmlNode casesInfo);
}