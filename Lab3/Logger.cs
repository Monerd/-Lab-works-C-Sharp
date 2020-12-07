using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Lab3
{
    class Logger
    {
        private readonly FileSystemWatcher watcher;
        public List<Folder> folders;
        private bool enabled = true;
        public Logger(ConfOpt opt, string errFile)
        {
            this.folders = opt.Folders;
            Archiving.ErrFile = errFile;
            Directory.CreateDirectory(folders.Find(f => f.Name == "LogFilePath").Path);
            Directory.CreateDirectory(folders.Find(f => f.Name == "Source").Path);
            Directory.CreateDirectory(folders.Find(f => f.Name == "Target").Path);
            watcher = new FileSystemWatcher(folders.Find(f => f.Name == "Source").Path);
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
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + Path.GetFileName(e.FullPath);
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving.Archive(e.FullPath, folders.Find(f => f.Name == "Target").Path);

        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving.Archive(filePath, folders.Find(f => f.Name == "Target").Path);
        }
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, false);
            Archiving.Archive(filePath, folders.Find(f => f.Name == "Target").Path);
        }
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath, true);
        }
        private void RecordEntry(string fileEvent, string filePath, bool delCheck)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(folders.Find(f => f.Name == "LogFilePath").Path, "templog.txt"), true))
            {
                if (delCheck)
                {
                    writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - файл {Path.GetFileName(filePath)} был {fileEvent}.");
                    writer.Flush();
                }
                else
                {
                    writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - файл {Path.GetFileName(filePath)} был {fileEvent} и архивирован.");
                    writer.Flush();
                }
            }
        }
    }
}
