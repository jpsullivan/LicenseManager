using LicenseManager.Authentication;
using LicenseManager.Models;
using LicenseManager.Services;

namespace LicenseManager.Infrastructure.Extensions
{
    /// <summary>
    /// Provides helper methods to generate credentials.
    /// </summary>
    public static class CredentialBuilder
    {
        public static Credential CreatePbkdf2Password(string plaintextPassword)
        {
            return new Credential(
                CredentialTypes.Password.Pbkdf2,
                CryptographyService.GenerateSaltedHash(plaintextPassword, Constants.PBKDF2HashAlgorithmId));
        }

        public static Credential CreateSha1Password(string plaintextPassword)
        {
            return new Credential(
                CredentialTypes.Password.Sha1,
                CryptographyService.GenerateSaltedHash(plaintextPassword, Constants.Sha1HashAlgorithmId));
        }

        internal static Credential CreateExternalCredential(string issuer, string value, string identity)
        {
            return new Credential(CredentialTypes.ExternalPrefix + issuer, value)
            {
                Ident = identity
            };
        }
    }
}