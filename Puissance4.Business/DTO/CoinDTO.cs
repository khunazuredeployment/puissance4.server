using Puissance4.Domain.Enums;

namespace Puissance4.Business.DTO
{
    public class CoinDTO
    {
        public CoinDTO(Guid gameId, int column, int height, Color color)
        {
            GameId = gameId;
            Column = column;
            Height = height;
            Color = color;
        }

        public Guid GameId { get; set; }
        public int Column { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }
    }
}
