using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DPK.Mosavabat.Common.Extensions;

public static class XmlExtensions
{
    /// <summary>
    /// تبدیل یک آبجکت به رشته XML تمیز (مناسب برای Bizagi)
    /// </summary>
    public static string ConvertObjectToXml<T>(
        this T objectToSerialize)
    {
        if (objectToSerialize == null)
        {
            return string.Empty;
        }

        try
        {
            var settings = new XmlWriterSettings
            {
                Indent = false,             // حذف فاصله‌های اضافی برای کاهش حجم
                OmitXmlDeclaration = true,  // حذف دکلریشن XML
                CheckCharacters = false     // جلوگیری از خطای کاراکترهای غیرمجاز خاص
            };

            var emptyNamespaces = new XmlSerializerNamespaces();
            emptyNamespaces.Add("", "");

            var serializer = new XmlSerializer(typeof(T));

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            serializer.Serialize(xmlWriter, objectToSerialize, emptyNamespaces);
            return stringWriter.ToString();
        }
        catch (Exception ex)
        {
            // در صورت بروز خطا در سریالایز، بهتر است لاگ شود یا خطای خوانا برگردد
            throw new InvalidOperationException(
                $"خطا در تبدیل مدل {typeof(T).Name} به XML",
                ex);
        }
    }
}