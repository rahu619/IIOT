using Newtonsoft.Json;
using System.Text;

namespace IIOT.MessageConsumer.Data.Extensions
{
    public static class SerializerExtension
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        public static byte[] Serialize<T>(this T source)
        {
            var content = JsonConvert.SerializeObject(source, SerializerSettings);
            return Encoding.UTF8.GetBytes(content);
        }

        public static T Deserialize<T>(this byte[] source)
        {
            var content = Encoding.UTF8.GetString(source);
            return JsonConvert.DeserializeObject<T>(content);
        }

    }
}
