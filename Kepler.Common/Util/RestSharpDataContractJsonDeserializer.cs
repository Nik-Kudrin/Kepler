using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace Kepler.Common.Util
{
    public class RestSharpDataContractJsonDeserializer : IDeserializer
    {
        /// Unused for JSON Serialization
        public string DateFormat { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            using (var ms = new MemoryStream(response.RawBytes))
            {
                var ser = new DataContractJsonSerializer(typeof (T));
                return (T) ser.ReadObject(ms);
            }
        }

        public T Deserialize<T>(string serializedObject)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject)))
            {
                var ser = new DataContractJsonSerializer(typeof (T));
                return (T) ser.ReadObject(ms);
            }
        }

        /// Unused for JSON Serialization
        public string RootElement { get; set; }

        /// Unused for JSON Serialization
        public string Namespace { get; set; }
    }
}