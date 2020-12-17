using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ServiceLayer
{
    public class Creator<T> : ICreator
    {
        private readonly IEnumerable<T> collection;
        private string strXML;
        private string strXSD;

        public Creator(IEnumerable<T> collection)
        {
            this.collection = collection;
        }

        private XElement ObjectToXml<T>(T obj)
        {
            XElement root = new XElement(typeof(T).Name);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                XElement element = new XElement(property.Name, property.GetValue(obj));
                root.Add(element);
            }
            return root;
        }

        public string CreateXML()
        {
            XElement products = new XElement("Products");
            foreach (var item in collection)
            {
                products.Add(ObjectToXml(item));
            }
            strXML = new XDeclaration("1.0", "utf-16", "yes").ToString() + "\n" + products.ToString();
            return strXML;
        }
        public string CreateXSD()
        {
            XmlReader reader = XmlReader.Create(new StringReader(strXML));
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();
            schemaSet = schema.InferSchema(reader);
            foreach (XmlSchema s in schemaSet.Schemas())
            {
                using var stringWriter = new StringWriter();
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using var writer = XmlWriter.Create(stringWriter, settings);
                s.Write(writer);
                strXSD = stringWriter.ToString();
            }
            return strXSD;
        }
    }
}
