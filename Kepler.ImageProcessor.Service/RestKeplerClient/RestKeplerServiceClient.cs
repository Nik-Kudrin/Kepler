using Kepler.Common.Error;
using Kepler.Common.Util;
using Kepler.ImageProcessor.Service.TaskManager;
using RestSharp;

namespace Kepler.ImageProcessor.Service.RestKeplerClient
{
    public class RestKeplerServiceClient
    {
        private string KeplerServiceUrl { get; set; }

        public RestKeplerServiceClient()
        {
            KeplerServiceUrl = TaskGenerator.KeplerServiceUrl;
        }

        public void LogError(string exceptionMessage)
        {
            var error = new ErrorMessage()
            {
                ExceptionMessage = exceptionMessage
            };

            var client = new RestClient(KeplerServiceUrl);
            var request = new RestRequest("LogError", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new RestSharpDataContractJsonSerializer();
            request.AddJsonBody(error);

            client.Execute(request);
        }
    }
}