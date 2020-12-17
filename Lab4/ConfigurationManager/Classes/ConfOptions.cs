using System;
using System.IO;

namespace MyConfigurationManager
{
    public class ConfOptions
    {
        private readonly IParser _configParser;
        public ConfOptions(IParser parser)
        {
            if (File.Exists(parser.GetPath()))
            {
                _configParser = parser;
            }
            else
            {
                throw new Exception("Error configuration!");
            }
        }
        public T GetOptions<T>() where T : new()
        {
            T element;
            element = _configParser.Parse<T>();
            return element;
        }
    }
}
