using System.Collections.Generic;
using System.Linq;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Models;
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


        public void StopStopDiffGeneration(List<ScreenShot> screenShotsToStopProcessing)
        {
            var client = new RestClient(_workerServiceUrl);
            var request = new RestRequest("StopDiffGeneration", Method.POST);

            request.RequestFormat = DataFormat.Json;
            var screenShots =
                screenShotsToStopProcessing.Select(item => new ImageComparisonInfo() {ScreenShotId = item.Id});

            request.AddBody(screenShots);
            client.Execute(request);
        }
    }
}