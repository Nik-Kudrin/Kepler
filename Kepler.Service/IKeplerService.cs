using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Core;

namespace Kepler.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IKeplerService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuild/{id}")]
        Build GetBuild(string id);

/*
        [OperationContract]
        IEnumerable<Build> GetBuilds();

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);*/

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "Kepler.Service.ContractType".
    [DataContract]
    public class CompositeType
    {
        private bool boolValue = true;
        private string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}