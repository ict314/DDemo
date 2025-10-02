namespace DDemo.Envelope
{
    internal static class Extensions
    {
        internal static Dictionary<string, string> ToDictionary(this string value)
        {
            Dictionary<string, string> recipients = [];
            try
            {
                foreach (string signer in value.Split(',', StringSplitOptions.TrimEntries))
                {
                    var parts = signer.Split(':', StringSplitOptions.TrimEntries);
                    if (parts.Length == 2)
                    {
                        string name = parts[0];
                        string email = parts[1];
                        if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
                        {
                            recipients[name] = email;
                        }
                    }
                }
            }
            catch
            {
                //Empty
            }
            return recipients;
        }
    }
}