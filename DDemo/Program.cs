using CommandLine;

namespace DDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => MainAsync(opts).GetAwaiter().GetResult());
        }
        private static async Task MainAsync(Options options)
        {
            string account_base_uri = "https://account-d.docusign.com";
            if (options.Environment == Environment.Production)
                account_base_uri = "https://account.docusign.com";

            OAuth.Key.Initialize(options.RSAPrivateKey);

            string accessToken = await OAuth.Token.Obtain(account_base_uri, options.IntegrationKey, options.UserId);
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ApplicationException($"invalid {accessToken}");

            string baseUri = await OAuth.UserInfo.BaseUri(account_base_uri, accessToken);
            if (string.IsNullOrWhiteSpace(baseUri))
                throw new ApplicationException($"invalid {baseUri}");

            Guid newEnvelopeId = await Envelope.Create.New(baseUri, options.Document, accessToken, options.ApiAccountId, options.Signers);
            if (newEnvelopeId != Guid.Empty)
                Console.WriteLine("new envelope created with id " + newEnvelopeId);
        }
    }
}
