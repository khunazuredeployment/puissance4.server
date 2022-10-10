using Puissance4.Domain.Enums;

namespace Puissance4.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public int? RedUserId { get; set; }
        public User? RedUser { get; set; }
        public int? YellowUserId { get; set; }
        public User? YellowUser { get; set; }
        public Color?[][] Grid { get; set; } = new Color?[7][];
    }
}
