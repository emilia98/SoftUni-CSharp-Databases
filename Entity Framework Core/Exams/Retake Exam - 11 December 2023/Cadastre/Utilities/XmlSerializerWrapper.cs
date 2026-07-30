using System.Text;
using System.Xml.Serialization;

namespace Cadastre.Utilities
{
    /* XmlSerializer class is used for both deserialization and serialization! */
    public static class XmlSerializerWrapper
    {
        public static T? Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);
            using StringReader stringReader = new StringReader(inputXml);

            T? result = (T?)xmlSerializer.Deserialize(stringReader);

            return result;
        }


        public static string Serialize<T>(T obj, string rootName, IDictionary<string, string>? xmlNamespaces = null)
        {
            StringBuilder result = new StringBuilder();

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            if (xmlNamespaces != null)
            {
                foreach (var xmlNamespacesKvp in xmlNamespaces)
                {
                    xmlns.Add(xmlNamespacesKvp.Key, xmlNamespacesKvp.Value);
                }
            }
            else
            {
                xmlns.Add(string.Empty, string.Empty);
            }

            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringWriter stringWriter = new StringWriter(result);

            xmlSerializer.Serialize(stringWriter, obj, xmlns);

            return result.ToString();
        }
    }
}
