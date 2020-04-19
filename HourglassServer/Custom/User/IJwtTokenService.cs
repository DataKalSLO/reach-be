using HourglassServer.Models.Persistent;

namespace HourglassServer
{
    public interface IJwtTokenService
    {
        string BuildToken(Person person);
    }
}
