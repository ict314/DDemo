using System.Net.Http.Headers;

namespace DDemo.OAuth
{
    internal static class UserInfo
    {
        internal static async Task<string> BaseUri(string baseUrl, string accessToken)
        {
            HttpClient httpClient = new() { BaseAddress = new Uri(baseUrl) };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using HttpResponseMessage response = await httpClient.GetAsync("oauth/userinfo");
            string reply = await response.Content.ReadAsStringAsync();

            ReplyObject replyObject = reply.Deserialize<ReplyObject>() ?? new ReplyObject();
            foreach (Account account in replyObject.Accounts)
            {
                if (account.Is_default)
                    return account.Base_uri;
            }

            return string.Empty;
        }
        private sealed class ReplyObject
        {
            public string Sub { get; init; } = string.Empty;
            public string Name { get; init; } = string.Empty;
            public string Given_name { get; init; } = string.Empty;
            public string Family_name { get; init; } = string.Empty;
            public DateTime Created { get; init; } = DateTime.MaxValue;
            public string Email { get; init; } = string.Empty;
            public Account[] Accounts { get; init; } = [];
        }
        private sealed class Account
        {
            public string Account_id { get; init; } = string.Empty;
            public bool Is_default { get; init; } = false;
            public string Account_name { get; init; } = string.Empty;
            public string Base_uri { get; init; } = string.Empty;
        }
    }
}