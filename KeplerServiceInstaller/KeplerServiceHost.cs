using System;
using System.ServiceModel;
using System.ServiceModel.Description;
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

            /*string strAdrHTTP = "http://localhost:9001/KeplerService";
            string strAdrTCP = "net.tcp://localhost:9002/KeplerService";

            Uri[] adrbase = {new Uri(strAdrHTTP), new Uri(strAdrTCP)};*/
            serviceHost = new ServiceHost(typeof (KeplerService) /*, adrbase*/);

            /*ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            serviceHost.Description.Behaviors.Add(mBehave);

            BasicHttpBinding httpb = new BasicHttpBinding();
            serviceHost.AddServiceEndpoint(typeof (IKeplerService), httpb, strAdrHTTP);
            serviceHost.AddServiceEndpoint(typeof (IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            NetTcpBinding tcpb = new NetTcpBinding();
            serviceHost.AddServiceEndpoint(typeof (IKeplerService), tcpb, strAdrTCP);
            serviceHost.AddServiceEndpoint(typeof (IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");*/


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