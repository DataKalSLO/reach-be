using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace HourglassServer
{
    public class PBKDF2PasswordHasher : PasswordHasher
    {
        private int iterations;

        public PBKDF2PasswordHasher(int iterations)
        {
            this.iterations = iterations;
        }

        protected override string Hash(string password, byte[] salt, int hashLength)
        {
            int hashByteCount = hashLength / 8;
            byte[] bytes = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, iterations, hashByteCount);
            return Convert.ToBase64String(bytes);
        }
    }
}
