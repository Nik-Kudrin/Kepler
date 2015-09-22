using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace KeplerImageProcessorInstaller
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            // InitializeComponent();
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "KeplerImageProcessorService";
            serviceInstaller.DisplayName = "KeplerImageProcessorService";
            serviceInstaller.Description = "Kepler Image Processor API Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}