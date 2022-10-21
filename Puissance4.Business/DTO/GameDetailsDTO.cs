using Puissance4.Domain.Entities;
using Puissance4.Domain.Enums;

namespace Puissance4.Business.DTO
{
    public class GameDetailsDTO : GameDTO
    {
        public Color?[][] Grid { get; set; }
        public string? WinnerUsername { get; set; }
        public int? WinnerId{ get; set; }
        public GameDetailsDTO(Game game) : base(game)
        {
            Grid = game.Grid;
            WinnerUsername = game.Winner?.Username;
            WinnerId = game.Winner?.Id;
        }
    }
}
