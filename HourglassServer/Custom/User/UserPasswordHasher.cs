namespace HourglassServer
{
    public static class UserPasswordHasher
    {
        private const int SaltLength = 16;
        private const int HashLength = 128;
        private const int HashIterations = 10000;
        private static readonly IPasswordHasher hasher = new PBKDF2PasswordHasher(HashIterations);

        public static (string salt, string hash) HashPassword(string password)
        {
            return hasher.HashPassword(password, SaltLength, HashLength);
        }

        public static bool PasswordMatches(string enteredPassword, string saltString, string realHash)
        {
            return hasher.PasswordMatches(enteredPassword, saltString, realHash, HashLength);
        }
    }
}
