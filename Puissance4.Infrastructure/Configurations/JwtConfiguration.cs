namespace Puissance4.Infrastructure.Configurations
{
    public class JwtConfiguration
    {
        public string Signature { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int LifeTime { get; set; } = 86400; // 1 day
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateLifeTime { get; set; }
    }
}
