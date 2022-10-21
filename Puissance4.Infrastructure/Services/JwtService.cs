using Microsoft.IdentityModel.Tokens;
using Puissance4.Business.Interfaces;
using Puissance4.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Puissance4.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtService(JwtConfiguration config, JwtSecurityTokenHandler tokenHandler)
        {
            _config = config;
            _tokenHandler = tokenHandler;
        }

        public string CreateToken(string identifier, string username)
        {
            DateTime now = DateTime.Now;
            JwtSecurityToken token = new(
                _config.Issuer,
                _config.Audience,
                CreateClaims(identifier, username),
                now,
                now.AddSeconds(_config.LifeTime),
                CreateCredentials()
            );

            return _tokenHandler.WriteToken(token);
        }

        private SigningCredentials CreateCredentials()
        {
            return new SigningCredentials(CreateKey(), SecurityAlgorithms.HmacSha256);
        }

        private SecurityKey CreateKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Signature));
        }

        private IEnumerable<Claim> CreateClaims(string identifier, string username)
        {
            yield return new Claim(ClaimTypes.NameIdentifier, identifier);
            yield return new Claim(ClaimTypes.Name, username);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                return _tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = _config.ValidateIssuer,
                    ValidateAudience = _config.ValidateAudience,
                    ValidateLifetime = _config.ValidateLifeTime,
                    ValidIssuer = _config.Issuer,
                    ValidAudience = _config.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Signature)),
                }, out SecurityToken secToken);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
