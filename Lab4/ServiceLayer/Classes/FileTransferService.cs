using System;
using System.Text;
using System.IO;

namespace ServiceLayer
{
    public class FileTransferService : IFileTransferService
    {
        private readonly string targetDirectory;
        private string pathForXML;
        private string pathForXSD;

        public FileTransferService(string targetDirectoryPath)
        {
            targetDirectory = targetDirectoryPath;
        }

        private Stream CreateFile(string content)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            MemoryStream memoryfile = new MemoryStream(byteArray)
            {
                Position = 0
            };
            return memoryfile;
        }

        private void CreatePaths()
        {
            pathForXML = Path.Combine(targetDirectory, "table") + ".xml";
            pathForXSD = Path.Combine(targetDirectory, "table") + ".xsd";
        }

        private void WriteFile(Stream source, string path)
        {
            using (var file = new FileStream(path, FileMode.Create))
            {
                source.CopyTo(file);
            }
        }

        public void FileTransfer(string contentXML, string contentXSD)
        {
            var xml = CreateFile(contentXML);
            var xsd = CreateFile(contentXSD);
            CreatePaths();
            WriteFile(xml, pathForXML);
            WriteFile(xsd, pathForXSD);
        }
    }
}
