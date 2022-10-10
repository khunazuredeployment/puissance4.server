using Puissance4.Business.DTO;
using Puissance4.Business.Exceptions;
using Puissance4.Domain.Entities;
using Puissance4.Domain.Enums;
namespace Puissance4.Business.Services
{
    public class GameService
    {
        public Dictionary<Guid, Game> Games { get; set; } = new Dictionary<Guid, Game>();

        public GameDTO Create(int userId, Color color)
        {
            Game game = new Game()
            {
                Id = Guid.NewGuid(),
            };
            if(color == Color.Red)
            {
                game.RedUserId = userId;
            }
            if (color == Color.Yellow)
            {
                game.YellowUserId = userId;
            }
            Games.Add(game.Id, game);
            return new GameDTO(game);
        }

        public GameDTO Join(Guid id, int userId)
        {
            Game game = Games[id];
            if (game.YellowUserId != null && game.RedUserId != null)
            {
                throw new GameException("Game is already full");
            }
            if (game.RedUserId == null)
            {
                game.RedUserId = userId;
            }
            else if(game.YellowUserId == null)
            {
                game.YellowUserId = userId;
            }
            return new GameDTO(game);
        }
    }
}
