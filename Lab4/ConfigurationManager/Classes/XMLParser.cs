using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MyConfigurationManager
{
    public class XMLParser : IParser
    {
        private readonly string xmlPath;
        public XMLParser(string xmlPath)
        {
            this.xmlPath = xmlPath;
        }
        public string GetPath()
        {
            return xmlPath;
        }
        public T Parse<T>() where T : new()
        {
            T obj = new T();
            try
            {
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
    }
}
