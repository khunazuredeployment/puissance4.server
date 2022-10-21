using System.Security.Claims;

namespace Puissance4.Business.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(string identifier, string username);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
