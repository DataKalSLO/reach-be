using System;
using System.Security.Cryptography;

namespace HourglassServer
{
    public abstract class PasswordHasher : IPasswordHasher
    {
        public (string salt, string hash) HashPassword(string password, int saltLength, int hashLength)
        {
            using var rngCsp = new RNGCryptoServiceProvider();
            byte[] salt = new byte[saltLength];
            rngCsp.GetBytes(salt);
            string hash = Hash(password, salt, hashLength);
            string saltString = Convert.ToBase64String(salt);
            return (saltString, hash);
        }

        public bool PasswordMatches(string enteredPassword, string saltString, string realHash, int hashLength)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            string hash = Hash(enteredPassword, salt, hashLength);
            return hash == realHash;
        }

        protected abstract string Hash(string password, byte[] salt, int hashLength);
    }
}
