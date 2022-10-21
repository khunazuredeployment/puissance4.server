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
        public bool RedIsConnected { get; set; }
        public bool YellowIsConnected { get; set; }
        public GameStatus Status { get; set; }
        public User? Winner { get; set; }
        public Color?[][] Grid { get; set; }
        public Game()
        {
            Grid = Enumerable.Range(0, 7).Select(x => Enumerable.Range(0, 6).Select(y => null as Color?).ToArray()).ToArray();
        }
    }
}
