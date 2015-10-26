using System.ComponentModel;
using System.ServiceProcess;

namespace Kepler.ImageProcessor.Installer
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            // InitializeComponent();
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "Kepler.ImageProcessor.Service";
            serviceInstaller.DisplayName = "Kepler.ImageProcessor.Service";
            serviceInstaller.Description = "Kepler Image Processor API Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}