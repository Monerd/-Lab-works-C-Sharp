using System;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace Lab3
{
    static class Archiving
    {
        public static string ErrFile { get; set; }
        public static void Archive(string fileName, string targetPath)
        {
            string path = Path.Combine(targetPath, DateTime.Now.ToString("dd MM yyyy HH mm ss"));
            Directory.CreateDirectory(path);
            string zipPath = Path.Combine(path, "Zip");
            Directory.CreateDirectory(zipPath);
            string orgPath = Path.Combine(path, "Encrypted File");
            Directory.CreateDirectory(orgPath);
            string fileNameZip = Path.Combine(Path.GetFileName(fileName), zipPath);
            try
            {
                FileInfo originalFile = new FileInfo(fileName);
                string name = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                FileStream tempFile = File.Create(Path.Combine(orgPath, name));
                tempFile.Close();
                string newFilePath = Path.Combine(orgPath, name);
                File.Copy(fileName, newFilePath, true);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (ZipArchive zip = new ZipArchive(memory, ZipArchiveMode.Create, true))
                    {
                        var memoryFile = zip.CreateEntry(Path.GetFileName(fileName), CompressionLevel.Optimal);
                        FileStream sourceStream = default;
                        while (true)
                        {
                            try
                            {
                                sourceStream = new FileStream(fileName, FileMode.Open);
                            }
                            catch (IOException)
                            {
                                continue;
                            }
                            break;
                        }

                        using (Stream targetEncryptedStream = memoryFile.Open())
                        {
                            Encryptor.Encrypt(sourceStream, targetEncryptedStream);
                        }
                        sourceStream.Close();
                        sourceStream.Dispose();
                    }
                    using (FileStream encryptedFileStream = new FileStream(Path.Combine(zipPath, Path.GetFileNameWithoutExtension(fileName) + ".zip"), FileMode.Create))
                    {
                        memory.Seek(0, SeekOrigin.Begin);
                        memory.CopyTo(encryptedFileStream);
                    }
                }
            }
            catch (Exception e)
            {
                using (StreamWriter err = new StreamWriter(new FileStream(ErrFile, FileMode.OpenOrCreate)))
                {
                    err.WriteLine($"{DateTime.Now.ToString("dd MM yyyy HH mm ss")} - произошла ошибка {e.Message}");
                    err.WriteLine(e.StackTrace);
                    err.Flush();
                }
            }
        }
    }
}