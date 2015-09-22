using System.ServiceModel;
using System.ServiceProcess;
using Kepler.Service;

namespace KeplerServiceInstaller
{
    public partial class KeplerServiceHost : ServiceBase
    {
        private ServiceHost serviceHost = null;

        public KeplerServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null) serviceHost.Close();

            serviceHost = new ServiceHost(typeof (KeplerService));
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}