using System;
using System.IO;
using System.Text.Json;

namespace Lab3
{
    class JSONParser : IParser
    {
        private readonly string jsonPath;
        public JSONParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }
        public T Parse<T>() where T : new()
        {
            T obj = new T();
            try
            {
                string jsonString = File.ReadAllText(jsonPath);
                obj = JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }
}
