using Puissance4.Domain.Entities;
using Puissance4.Domain.Enums;

namespace Puissance4.Business.DTO
{
    public class GameDTO
    {
        public Guid Id { get; set; }
        public string? RedUsername { get; set; }
        public string? YellowUsername { get; set; }
        public Color?[][] Grid { get; set; } = new Color?[7][];

        public GameDTO(Game g)
        {
            Id = g.Id;
            RedUsername = g.RedUser?.Username;
            YellowUsername = g.YellowUser?.Username;
            Grid = g.Grid;
        }
    }
}
