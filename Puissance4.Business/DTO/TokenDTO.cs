namespace Puissance4.Business.DTO
{
    public class AuthenticationDTO
    {
        public string Token { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
