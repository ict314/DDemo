using System.Text.Json;

namespace DDemo.OAuth
{
    internal static class Extensions
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        internal static T? Deserialize<T>(this string value)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(value, jsonOptions);
            }
            catch { }
            return default;
        }
    }
}
