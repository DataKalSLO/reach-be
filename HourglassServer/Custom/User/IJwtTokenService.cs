using HourglassServer.Models.Persistent;
using System.Security.Claims;

namespace HourglassServer
{
    public interface IJwtTokenService
    {
        string BuildToken(Claim[] claims);
    }
}
