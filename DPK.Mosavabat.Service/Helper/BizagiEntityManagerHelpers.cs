using System.Xml;
using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Shared.Templates.Bizagi.Base;
using DPK.Mosavabat.Shared.Templates.Bizagi.Base2;
using DPK.Mosavabat.Shared.Templates.Bizagi.EntityManager.SaveEntity;

namespace DPK.Mosavabat.Services.Helper;

public class BizagiEntityManagerHelpers
{
    public static BizAgiWSParamUpdate2<T> GenerateSaveActivityDataModel<T>(T model)
    {
        return new BizAgiWSParamUpdate2<T>
        {
            Entities = new Entities2<T>
            {
                Dynamic = model
            }
        };
    }

    // ساخت مدل برای جستجو
    public static BizAgiWsParam CreateFilterModel(string entityName, string caseId)
    {
        return new BizAgiWsParam
        {
            EntityData = new EntityData
            {
                EntityName = entityName,
                Filters = $"<![CDATA[CaseId = {caseId}]]>"
            }
        };
    }

    public static string ConvertModelToXmlMosavabat<T>(T model,string mosavabat)
    {
        var xml = model.ConvertObjectToXml();
        return xml.Replace(nameof(Entities<int>.Dynamic), mosavabat.ToString());
    }

    // یک فایل اکس ام ال را می خواند
    public static XmlDocument ReadXmlFromFile(string filePath)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        return xmlDoc;
    }

}