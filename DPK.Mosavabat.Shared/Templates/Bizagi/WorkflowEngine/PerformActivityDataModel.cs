using System.Xml.Serialization;

namespace DPK.Mosavabat.Shared.Templates.Bizagi.WorkflowEngine
{
    public class PerformActivityDataModel
    {
		// using System.Xml.Serialization;
        // XmlSerializer serializer = new XmlSerializer(typeof(Schema));
        // using (StringReader reader = new StringReader(xml))
        // {
        //    var test = (Schema)serializer.Deserialize(reader);
        // }

        [XmlRoot(ElementName = "element")]
        public class Element
        {

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "type")]
            public string Type { get; set; }

            [XmlElement(ElementName = "complexType")]
            public ComplexType ComplexType { get; set; }
        }

        [XmlRoot(ElementName = "choice")]
        public class Choice
        {

            [XmlElement(ElementName = "element")]
            public List<Element> Element { get; set; }
        }

        [XmlRoot(ElementName = "sequence")]
        public class Sequence
        {

            [XmlElement(ElementName = "choice")]
            public List<Choice> Choice { get; set; }

            [XmlElement(ElementName = "element")]
            public List<Element> Element { get; set; }
        }

        [XmlRoot(ElementName = "complexType")]
        public class ComplexType
        {

            [XmlElement(ElementName = "sequence")]
            public Sequence Sequence { get; set; }
        }

        [XmlRoot(ElementName = "schema")]
        public class Schema
        {

            [XmlElement(ElementName = "element")]
            public Element Element { get; set; }

            [XmlAttribute(AttributeName = "xs")]
            public string Xs { get; set; }

            [XmlAttribute(AttributeName = "elementFormDefault")]
            public string ElementFormDefault { get; set; }

            [XmlAttribute(AttributeName = "attributeFormDefault")]
            public string AttributeFormDefault { get; set; }
        }


	}
}
