using HourglassServer.Data;

namespace HourglassServer
{
    public interface IJwtTokenService
    {
        string BuildToken(Person person);
    }
}
