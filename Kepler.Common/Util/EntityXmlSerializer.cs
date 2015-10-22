using System;
using System.IO;
using System.Xml.Serialization;

namespace Kepler.Common.Util
{
    public static class EntityXmlSerializer
    {
        public static string Serialize(object entity)
        {
            string xmlString = String.Empty;

            var serializer = new XmlSerializer(entity.GetType());
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, entity);
                writer.Flush();
                xmlString = writer.ToString();
            }

            return xmlString;
        }

        public static T DeserializeFromString<T>(string xmlString)
        {
            return (T) DeserializeFromString(xmlString, typeof (T));
        }


        public static object DeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}