using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Kepler.HostService
{
    [RunInstaller(true)]
    public partial class KeplerHostServiceInstaller : Installer
    {
        public KeplerHostServiceInstaller()
        {
            // InitializeComponent();
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "KeplerService";
            serviceInstaller1.DisplayName = "KeplerService";
            serviceInstaller1.Description = "Kepler API Service";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
    }
}