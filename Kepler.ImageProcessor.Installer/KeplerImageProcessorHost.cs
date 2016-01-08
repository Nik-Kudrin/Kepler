using System.ServiceModel;
using System.ServiceProcess;
using Kepler.ImageProcessor.Service;

namespace Kepler.ImageProcessor.Installer
{
    public partial class KeplerImageProcessorHost : ServiceBase
    {
        private ServiceHost serviceHost = null;

        public KeplerImageProcessorHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null) serviceHost.Close();

            serviceHost = new ServiceHost(typeof (KeplerImageProcessorService));
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