using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Kepler.ServiceInstaller
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            // InitializeComponent();
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            serviceInstaller.ServiceName = "Kepler.Service";
            serviceInstaller.DisplayName = "Kepler.Service";
            serviceInstaller.Description = "Kepler API Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
        }
    }
}