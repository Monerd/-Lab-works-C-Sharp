using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Reflection;

namespace MyConfigurationManager
{
    public class JSONParser : IParser
    {
        private readonly string jsonPath;
        public string GetPath()
        {
            return jsonPath;
        }
        public JSONParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public T Parse<T>() where T : new()
        {
            T element;
            try
            {
                element = (T)Activator.CreateInstance<T>();
            }
            catch (Exception)
            {
                return new T();
            }
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            string jsonPath = ConfigurationManager.AppSettings.Get("path");
            string jsonText = File.ReadAllText(jsonPath);

            using (JsonDocument jsonDoc = JsonDocument.Parse(jsonText))
            {
                JsonElement jsonRoot = jsonDoc.RootElement;
                if (jsonRoot.ValueKind != JsonValueKind.Null && jsonRoot.ValueKind != JsonValueKind.Undefined)
                {
                    foreach (JsonProperty field in jsonRoot.EnumerateObject())
                    {
                        if (field.Value.ValueKind != JsonValueKind.Null && field.Value.ValueKind != JsonValueKind.Undefined)
                        {
                            foreach (PropertyInfo info in propertyInfos)
                            {
                                if (info.Name == field.Name)
                                {
                                    info.SetValue(element, Convert.ChangeType(field.Value.ToString(), info.PropertyType), null);
                                }
                            }
                        }
                    }
                    return element;
                }
            }
            return new T();
        }
    }
}