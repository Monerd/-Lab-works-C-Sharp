using System;
using System.ServiceProcess;
using System.Threading;
using System.IO;

namespace Lab3
{
    public partial class FileWatcherService : ServiceBase
    {
        private Logger logger;
        private ConfOpt opt;
        public FileWatcherService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration.json"));
                opt = parser.Parse<ConfOpt>();
                logger = new Logger(opt, "ErrorLog.txt");
                Thread loggerThread = new Thread(new ThreadStart(logger.Start));
                loggerThread.Start();
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt"), FileMode.OpenOrCreate)))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} - Произошла ошибка! {e.Message}.");
                    writer.WriteLine($"{e.StackTrace}");
                    writer.Flush();
                }
            }
        }
        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
