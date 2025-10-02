using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace DDemo.OAuth
{
    internal class Key
    {
        internal static RsaSecurityKey? RsaSecurityKey;

        internal static void Initialize(string keyPath)
        {
            RsaSecurityKey = LoadKey(keyPath);
        }

        private static RsaSecurityKey LoadKey(string keyPath)
        {
            if (!File.Exists(keyPath))
                throw new ApplicationException($"{keyPath} not found");

            RSA rsaKey = RSA.Create();
            string pem = File.ReadAllText(keyPath);
            rsaKey.ImportFromPem(pem);
            return new RsaSecurityKey(rsaKey);
        }
    }
}
