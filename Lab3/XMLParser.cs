using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Lab3
{
    class XMLParser : IParser
    {
        private readonly string xmlPath;
        public XMLParser(string xmlPath)
        {
            this.xmlPath = xmlPath;
        }
        public T Parse<T>() where T : new()
        {
            T obj = new T();
            try
            {
                Affirm();
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var fs = new FileStream(xmlPath, FileMode.OpenOrCreate))
                {
                    obj = (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        private void Affirm()
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            var affirmPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Affirm.xsd");
            schemas.Add(null, affirmPath);
            XDocument doc = XDocument.Load(xmlPath);
            doc.Validate(schemas, (sender, validationEventArgs) =>
            {
                throw validationEventArgs.Exception;
            });
        }
    }
}
