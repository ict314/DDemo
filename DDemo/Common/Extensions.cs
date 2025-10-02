using System.Text.Json;

namespace DDemo
{
    internal static class Extensions
    {
        internal static string Serialize(this object value)
        {
            try
            {
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                JsonSerializerOptions jso = jsonSerializerOptions;

                return JsonSerializer.Serialize(value, jso);
            }
            catch { }
            return string.Empty;
        }
    }
}
