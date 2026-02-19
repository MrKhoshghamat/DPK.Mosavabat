using System.Xml;
using DPK.Foundation.Dependency;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Xml;

public interface IBizagiXmlBuilder
    : ITransientDependency
{
    XmlNode? ConvertObjectToXml<T>(T obj);

    XmlNode? BuildCreateCaseXml<TModel>(
        TModel model,
        string userName,
        string processName,
        string entityName);

    XmlNode? BuildSaveEntityXml<TModel>(
        TModel model,
        string entityName);
}