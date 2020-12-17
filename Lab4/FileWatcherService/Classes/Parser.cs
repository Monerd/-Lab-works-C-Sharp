using System;
using System.IO;

namespace Lab3
{
    public class Parser
    {
        private readonly IParser parser;
        public Parser(string fileNamePars)
        {
            if (File.Exists(fileNamePars) && Path.GetExtension(fileNamePars) == ".xml")
            {
                parser = new XMLParser(fileNamePars);
            }
            else
            {
                if (File.Exists(fileNamePars) && Path.GetExtension(fileNamePars) == ".json")
                {
                    parser = new JSONParser(fileNamePars);
                }
            }
        }
        public T Parse<T>() where T : new()
        {
            try
            {
                return parser.Parse<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
