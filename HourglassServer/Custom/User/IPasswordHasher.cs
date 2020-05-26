using System;

namespace HourglassServer
{
    public interface IPasswordHasher
    {
        (string salt, string hash) HashPassword(string password, int saltLength, int hashLength);

        bool PasswordMatches(string enteredPassword, string saltString, string realHash, int hashLength);
    }
}
