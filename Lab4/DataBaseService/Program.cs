using System.ServiceProcess;

namespace DataBaseService
{
    static class Program
    {
        static void Main()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DataBaseService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
