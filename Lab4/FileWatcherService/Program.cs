using System.Diagnostics;
using System.ServiceProcess;

namespace Lab3
{
    static class Program
    {
        static void Main()
        {
#if DEBUG
            Debugger.Launch();
#endif
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
