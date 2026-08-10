using System.Xml;
using System.Xml.Serialization;

namespace DPK.Mosavabat.Services.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// مدل را به xml تبدیل می کند
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string ConvertObjectToXml<T>(this T model)
    {
        var xsSubmit = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter);
        xsSubmit.Serialize(xmlWriter, model);
        return stringWriter.ToString(); // Your XML
    }

    public static bool IsNullObject<T>(this T model)
    {
        return model.GetType().GetProperties()
            .All(p => p.GetValue(model) == null);
    }
}