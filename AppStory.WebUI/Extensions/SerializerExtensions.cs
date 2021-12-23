using Newtonsoft.Json;

namespace AppStory
{
    public static class SerializerExtensions
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }
    }
}
