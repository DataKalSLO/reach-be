using HourglassServer.Models.Persistent;

namespace HourglassServer
{
    public interface IJwtTokenService
    {
        string BuildLoginToken(Person person);
        string BuildPasswordResetToken(string email);
    }
}
