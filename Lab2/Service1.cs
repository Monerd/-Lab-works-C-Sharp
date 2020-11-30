using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace FileWathcerService
{
    public partial class Service1 : ServiceBase
    {
        Logger logger;
        public Service1()
        {
            // Возможность останавливать/приостанавливать/возобновлять и вести лог сервиса
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }


        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
    }
    // Создание логера для отслеживания действий в папке ( удаление/создание/изменение/переименование файлов )
    class Logger
    {
        string targetPath = @"D:\\!Lab2\\TargetDirectory";
        FileSystemWatcher watcher;
        object obj = new object();
        bool enabled = true;
        public Logger()
        {
            watcher = new FileSystemWatcher("D:\\!Lab2\\SourceDirectory");
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }
        // Действия при переименовании файла
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving(e.FullPath);
            
        }
        // Действия при изменении файла
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving(filePath);
        }
        // Действия при создании файла
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving(filePath);
        }
        // Действия при удалении файла
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, true);
        }
        // Копирование и архивация файлов, созданных в SourceDirectory
        private void Archiving(string filePath)
        {
            // Создание в TargetDirectory папки с датой создания файла в SourceDirectory
            string path = Path.Combine(targetPath, DateTime.Now.ToString("dd MM yyyy HH mm ss"));
            Directory.CreateDirectory(path);
            // Создание в уже созданной по дате папке новой папки с архивированных файлом
            string zipPath = Path.Combine(path, "Zip");
            Directory.CreateDirectory(zipPath);
            // Создание в уже созданной по дате папке новой папки с исходным ( копированным ) файлом
            string orgPath = Path.Combine(path, "Original");
            Directory.CreateDirectory(orgPath);
            try
            {
                FileInfo originalFile = new FileInfo(filePath);
                string name = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                //Копирование файла
                FileStream tempFile = File.Create(Path.Combine(orgPath, name));
                tempFile.Close();
                string newFilePath = Path.Combine(orgPath, name);
                File.Copy(filePath, newFilePath, true);
                // Изменение типа файла
                name += ".gz";
                // Создание архивированного файла
                FileStream tempZip = File.Create(Path.Combine(zipPath, name));
                tempZip.Close();
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    using (FileStream zipFile = new FileStream(Path.Combine(zipPath, name), FileMode.Open))
                    {
                        // Сжатие содержимого в архивированном файле
                        using (GZipStream sevenZip = new GZipStream(zipFile, CompressionMode.Compress))
                        {
                            file.CopyTo(sevenZip);
                        }
                    }
                }
            }
            catch (Exception e) // Оповещение об ошибке работы программы в случае какого-либо сбоя в ходе копирования и архивирования
            {
                using (StreamWriter writer = new StreamWriter("D:\\!Lab2\\templog.txt", true))
                {
                    writer.WriteLine($"Произошла ошибка! {e.Message} {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}.");
                    writer.WriteLine($"Советуем проверить работу службы, возможна остановка!");
                    writer.Flush();
                }
                throw;
            }
        }
        private void RecordEntry(string fileEvent, string filePath, bool delCheck)
        {
            // Синхронизация потока ( чтобы любой другой поток не имел доступа к объекту )
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("D:\\!Lab2\\templog.txt", true))
                {
                    if (delCheck)
                    {
                        writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - файл {filePath} был {fileEvent}.");
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - файл {filePath} был {fileEvent} и архивирован.");
                        writer.Flush();
                    }
                }
            }
        }
    }
}
