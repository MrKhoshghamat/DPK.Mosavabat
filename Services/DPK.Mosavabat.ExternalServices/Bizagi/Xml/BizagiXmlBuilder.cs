using System.Text;
using System.Xml;
using DPK.Foundation.UI;
using System.Xml.Serialization;
using DPK.Mosavabat.ExternalServices.Bizagi.Templates.Abort;
using DPK.Mosavabat.ExternalServices.Bizagi.Templates.Base;
using Microsoft.Extensions.Logging;

namespace DPK.Mosavabat.ExternalServices.Bizagi.Xml;

public class BizagiXmlBuilder(
    ILogger<BizagiXmlBuilder> logger)
    : IBizagiXmlBuilder
{
    private const string DefaultDomain = "srv-mosavabat";

    public XmlNode? ConvertObjectToXml<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        try
        {
            var emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "");

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                Encoding = new UTF8Encoding(false)
            };

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(T));

            using (var xmlWriter = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(xmlWriter, obj, emptyNamespaces);
            }

            var xmlString = sb.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            return doc.DocumentElement;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error serializing object of type {Type}", typeof(T).Name);
            throw new UserFriendlyException("خطا در آماده‌سازی داده‌ها برای ارسال.");
        }
    }

    public XmlNode? BuildCreateCaseXml<TModel>(
        TModel model,
        string userName,
        string processName,
        string entityName)
    {
        var wrapper = new BizagiWsParamCreate<TModel>
        {
            Domain = DefaultDomain,
            UserName = userName,
            Cases = new Cases<TModel>
            {
                Case = new Case<TModel>
                {
                    Process = processName,
                    Entities = new Entities<TModel>
                    {
                        Dynamic = model
                    }
                }
            }
        };

        var xmlNode = ConvertObjectToXml(wrapper);

        if (string.IsNullOrEmpty(entityName) || entityName == "Dynamic") return xmlNode;

        var entitiesNode = xmlNode?.SelectSingleNode("//Entities");
        var dynamicNode = entitiesNode?.SelectSingleNode("Dynamic");

        if (dynamicNode != null && entitiesNode != null)
        {
            var newElement = xmlNode?.OwnerDocument?.CreateElement(entityName);

            if (newElement == null) return xmlNode;
            newElement.InnerXml = dynamicNode.InnerXml;

            entitiesNode.ReplaceChild(newElement, dynamicNode);
        }
        else
        {
            logger.LogWarning("DOM manipulation failed for entity renaming. Falling back to string replacement.");
            var xmlStr = xmlNode?.OuterXml.Replace("<Dynamic>", $"<{entityName}>")
                .Replace("</Dynamic>", $"</{entityName}>");

            var newDoc = new XmlDocument();
            if (xmlStr != null) newDoc.LoadXml(xmlStr);
            return newDoc.DocumentElement;
        }

        return xmlNode;
    }

    public XmlNode? BuildSaveEntityXml<TModel>(
        TModel model,
        string entityName)
    {
        var wrapper = new BizagiWsParamEntity<TModel>
        {
            Entities = new Entities<TModel>
            {
                Dynamic = model
            }
        };

        var xmlNode = ConvertObjectToXml(
            wrapper);

        return RenameDynamicNode(
            xmlNode,
            entityName);
    }

    private XmlNode? RenameDynamicNode(
        XmlNode? xmlNode,
        string entityName)
    {
        if (xmlNode == null || string.IsNullOrEmpty(entityName) || entityName == "Dynamic")
            return xmlNode;

        var entitiesNode = xmlNode.SelectSingleNode("//Entities");
        var dynamicNode = entitiesNode?.SelectSingleNode("Dynamic");

        if (dynamicNode != null && entitiesNode != null)
        {
            var newElement = xmlNode.OwnerDocument?.CreateElement(entityName);
            if (newElement == null) return xmlNode;

            newElement.InnerXml = dynamicNode.InnerXml;

            if (dynamicNode.Attributes != null)
                foreach (XmlAttribute attr in dynamicNode.Attributes)
                {
                    newElement.SetAttribute(attr.Name, attr.Value);
                }

            entitiesNode.ReplaceChild(newElement, dynamicNode);
        }
        else
        {
            logger.LogWarning("DOM manipulation failed for entity renaming to {Entity}. Falling back to string replacement.", entityName);
            var xmlStr = xmlNode.OuterXml
                .Replace("<Dynamic>", $"<{entityName}>")
                .Replace("</Dynamic>", $"</{entityName}>")
                .Replace("<Dynamic ", $"<{entityName} ");

            var newDoc = new XmlDocument();
            newDoc.LoadXml(xmlStr);
            return newDoc.DocumentElement;
        }

        return xmlNode;
    }
}