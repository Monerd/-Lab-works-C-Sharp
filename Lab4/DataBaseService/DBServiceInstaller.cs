using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DataBaseService
{
    [RunInstaller(true)]
    public partial class DBServiceInstaller : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        public DBServiceInstaller()
        {
            InitializeComponent();
            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "DataBase Service";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}