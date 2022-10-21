namespace Puissance4.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] Password { get; set; } = Array.Empty<byte>();
        public Guid Salt { get; set; }
    }
}
