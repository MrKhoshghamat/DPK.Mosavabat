using DPK.Mosavabat.Shared.Templates.Bizagi;
using System.Xml;

namespace DPK.Mosavabat.External.Services.Contracts.Bizagi;

public interface IEntityManagerServices
{
    Task<MosavabatResponseDataModel?> SaveEntityAsync(XmlNode casesInfo);
    Task<MosavabatResponseDataModel?> GetCaseDataUsingSchemaAsync(XmlNode casesInfo);
    Task<MosavabatResponseDataModel?> GetEntitiesAsync(XmlNode casesInfo);
    Task<string?> GetEntitiesUsingSchemaAsync(XmlNode entitiesInfo, XmlNode schema);
}