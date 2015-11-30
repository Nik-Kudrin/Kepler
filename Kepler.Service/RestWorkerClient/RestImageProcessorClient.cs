using System.Collections.Generic;
using System.Linq;
using System.Net;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Models;
using Kepler.Service.Core;
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

        public void SetKeplerServiceUrl()
        {
            var client = new RestClient(_workerServiceUrl);
            var request = new RestRequest("SetKeplerServiceUrl", Method.GET);

            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("url", BuildExecutor.KeplerServiceUrl);
            var response = client.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed ||
                !(response.StatusCode == HttpStatusCode.OK ||
                  response.StatusCode == HttpStatusCode.MultipleChoices ||
                  response.StatusCode == HttpStatusCode.Ambiguous ||
                  response.StatusCode == HttpStatusCode.MovedPermanently ||
                  response.StatusCode == HttpStatusCode.Moved ||
                  response.StatusCode == HttpStatusCode.Found ||
                  response.StatusCode == HttpStatusCode.Redirect ||
                  response.StatusCode == HttpStatusCode.SeeOther ||
                  response.StatusCode == HttpStatusCode.RedirectMethod ||
                  response.StatusCode == HttpStatusCode.NotModified))
            {
                throw new WebException(response.ErrorMessage);
            }
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