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

		private static JsonSerializerSettings _settingsIndent = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented,
		};

		public static T? FromJson<T>(this string json)
		{
			return JsonConvert.DeserializeObject<T>(json, _settings);
		}

		public static string ToJson<T>(this T obj)
		{
			return JsonConvert.SerializeObject(obj, _settings);
		}

		public static string ToJsonIndent<T>(this T obj)
		{
			return JsonConvert.SerializeObject(obj, _settingsIndent);
		}
	}
}
