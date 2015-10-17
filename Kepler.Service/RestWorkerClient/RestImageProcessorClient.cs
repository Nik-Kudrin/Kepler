using RestSharp;

namespace Kepler.Service.RestWorkerClient
{
    public class RestImageProcessorClient
    {
        private string _workerServiceUrl;

        public RestImageProcessorClient(string workerServiceUrl)
        {
            _workerServiceUrl = workerServiceUrl;
        }

        public void SetDiffImagePath()
        {
            var client = new RestClient(_workerServiceUrl);
            var request = new RestRequest("SetKeplerServiceUrl", Method.GET);

            request.RequestFormat = DataFormat.Json;

            var url = System.ServiceModel.OperationContext.Current.Host.BaseAddresses[0];
            request.AddQueryParameter("url", url.AbsoluteUri);
            client.Execute(request);
        }
    }
}