using CommandLine;

namespace DDemo
{
    internal sealed class Options
    {
        [Option('i', "IntegrationKey", Required = true, HelpText = "Integration Key, GUID value that identifies your integration")]
        public Guid IntegrationKey { get; init; } = Guid.Empty;

        [Option('u', "UserId", Required = true, HelpText = "User Id, GUID of the user to impersonate")]
        public Guid UserId { get; init; } = Guid.Empty;

        [Option('a', "ApiAccountId", Required = true, HelpText = "Api Account Id, GUID of application account")]
        public Guid ApiAccountId { get; init; } = Guid.Empty;

        [Option('p', "RSAPrivateKey", Required = true, HelpText = "RSA Private key file path")]
        public string RSAPrivateKey { get; init; } = string.Empty;

        [Option('s', "Signers", Required = true, HelpText = "in the format '1st signer Name:Email,2nd Name:Email'")]
        public string Signers { get; init; } = string.Empty;

        [Option('d', "Document", Required = true, HelpText = "Document to sign")]
        public string Document { get; init; } = string.Empty;

        [Option('e', "Environment", Required = false, HelpText = "Development (default) or Production")]
        public Environment Environment { get; init; } = Environment.Development;
    }
    internal enum Environment
    {
        Development,
        Production
    }
}
