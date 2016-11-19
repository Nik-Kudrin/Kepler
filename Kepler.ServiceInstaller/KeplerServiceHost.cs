using System;
using System.ServiceModel;
using System.ServiceProcess;
using Kepler.Service;

namespace Kepler.ServiceInstaller
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
            serviceHost.Faulted += new EventHandler(myHost_Faulted);
            serviceHost.Open();
        }

        private void myHost_Faulted(object sender, EventArgs e)
        {
            EventLog.WriteEntry($"Kepler.Service host faulted.  {e.ToString()} ");
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                try
                {
                    serviceHost.Close();
                }
                catch (Exception)
                {
                    try
                    {
                        serviceHost.Abort();
                    }
                    catch
                    {
                    }
                }

                serviceHost = null;
            }
        }
    }
}