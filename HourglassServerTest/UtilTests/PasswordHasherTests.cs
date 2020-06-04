using Microsoft.VisualStudio.TestTools.UnitTesting;
using HourglassServer;

// A class which trivially implements `Hash` by returning the given password.
class PlaintextPasswordHasher : PasswordHasher
{
    protected override string Hash(string password, byte[] salt, int hashLength)
    {
        return password;
    }
}

namespace HourglassServerTest
{
    [TestClass]
    public class PasswordHasherTests
    {
        [TestMethod]
        public void TestRoundtripHashingPBKDF2()
        {
            int iterations = 100;
            TestRoundtripHashing(new PBKDF2PasswordHasher(iterations));
        }

        [TestMethod]
        public void TestRoundtripHashingPlaintext()
        {
            TestRoundtripHashing(new PlaintextPasswordHasher());
        }

        // For a given `IPasswordHasher`, over a series of test passwords:
        // 1. Hash the test password, generating a unique salt.
        // 2. Validate that the newly hashed password, when tested using the generated salt, matches the original password.
        private void TestRoundtripHashing(IPasswordHasher hasher)
        {
            int saltLength = 16;
            int hashLength = 64;
            var testPasswords = new string[] {
                "", "1", "short", "s p a c e d o u t",
                "!!@@sp3c14Lch4R4Ct3Rs##$$%", "\"quoted\"",
                "reeeeeeeeeeeeeeeeeaaaaaaaaaaaaaaaaaallllllllllllllllyyyyyyyyyy_____looooonnnggggggg"
            };

            foreach(var testPassword in testPasswords)
            {
                var (salt, hash) = hasher.HashPassword(testPassword, saltLength, hashLength);
                Assert.IsTrue(hasher.PasswordMatches(testPassword, salt, hash, hashLength));
            }
        }
    }
}
