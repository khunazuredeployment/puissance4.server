using Puissance4.Domain.Entities;
using Puissance4.Domain.Enums;

namespace Puissance4.Business.DTO
{
    public class GameDTO
    {
        public Guid Id { get; set; }
        public GameStatus Status { get; set; }
        public int? RedUserId { get; set; }
        public int? YellowUserId{ get; set; }
        public string? RedUsername { get; set; }
        public string? YellowUsername { get; set; }
        public bool RedIsConnected{ get; set; }
        public bool YellowIsConnected { get; set; }

        public GameDTO(Game game)
        {
            Id = game.Id;
            RedUserId = game.RedUserId;
            YellowUserId = game.YellowUserId;
            RedUsername = game.RedUser?.Username;
            YellowUsername = game.YellowUser?.Username;
            RedIsConnected = game.RedIsConnected;
            YellowIsConnected = game.YellowIsConnected;
            Status = game.Status;
        }
    }
}
