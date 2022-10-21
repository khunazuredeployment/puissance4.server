using Puissance4.Business.DTO;
using Puissance4.Business.Exceptions;
using Puissance4.Business.Interfaces;
using Puissance4.Domain.Entities;
using Puissance4.Domain.Enums;
namespace Puissance4.Business.Services
{
    public class GameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public GameService(IGameRepository gameRepository, IUserRepository userRepository)
        {
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<GameDTO> GetAll()
        {
            return _gameRepository.GetAll().Select(g => new GameDTO(g));
        }

        public GameDetailsDTO? GetById(Guid gameId)
        {
            Game? g = _gameRepository.GetById(gameId);
            if(g is null)
            {
                return null;
            }
            return new GameDetailsDTO(g);
        }

        public GameDetailsDTO? GetCurrentGame(int userId)
        {
            Game? g = _gameRepository.GetByPlayer(userId);
            if (g is null)
            {
                return null;
            }
            return new GameDetailsDTO(g);
        }


        public async Task<GameDetailsDTO> Create(int userId, Color color)
        {
            Game game = new()
            {
                Id = Guid.NewGuid(),
            };
            if(color == Color.Red)
            {
                game.RedUserId = userId;
                game.RedIsConnected = true;
                game.RedUser = await _userRepository.FindByUserIdAsync(userId);
            }
            if (color == Color.Yellow)
            {
                game.YellowUserId = userId;
                game.YellowIsConnected = true;
                game.YellowUser = await _userRepository.FindByUserIdAsync(userId);
            }
            _gameRepository.Add(game);
            return new GameDetailsDTO(game);
        }

        public (CoinDTO, GameDetailsDTO) Play(int userId, Guid gameId, int column)
        {
            Game? game = _gameRepository.GetById(gameId);
            if (game is null)
            {
                throw new GameException("Partie introuvable");
            }
            if (game.Status == GameStatus.WaitingForPlayers)
            {
                throw new GameException("Vous ne pouvez pas encore jouer");
            }
            if (game.Status == GameStatus.Closed)
            {
                throw new GameException("La partie est terminée");
            }
            Color color = game.Grid.Sum(col => col.Count(t => t != null)) % 2 == 0 ? Color.Red : Color.Yellow;
            if ((color == Color.Red && game.RedUserId != userId) || (color == Color.Yellow && game.YellowUserId != userId))
            {
                throw new GameException("Ce n'est pas à vous de jouer");
            }
            int height = game.Grid[column].Count(t => t != null);
            if(height >= 6)
            {
                throw new GameException("La colonne est remplie");
            }
            game.Grid[column][height] = color;

            if (IsFull(game.Grid))
            {
                game.Status = GameStatus.Closed;
            }

            if (CheckWin(game.Grid, column, height, color))
            {
                game.Status = GameStatus.Closed;
                game.Winner = userId == game.RedUserId ? game.RedUser : game.YellowUser;
            }

            return (new CoinDTO(game.Id, column, height, color), new GameDetailsDTO(game));
        }

        public async Task<GameDetailsDTO> Join(int userId, Guid gameId)
        {
            Game? game = _gameRepository.GetById(gameId);
            if (game is null)
            {
                throw new GameException("Partie introuvable");
            }
            if (game.YellowUserId == userId || game.RedUserId == userId)
            {
                throw new GameException("Vous êtes déjà dans cette partie");
            }
            if (game.YellowUserId != null && game.RedUserId != null)
            {
                throw new GameException("La partie a déjà commencé");
            }
            if (game.RedUserId == null)
            {
                game.RedUserId = userId;
                game.RedIsConnected = true;
                game.RedUser = await _userRepository.FindByUserIdAsync(userId);
            }
            else if(game.YellowUserId == null)
            {
                game.YellowUserId = userId;
                game.YellowIsConnected = true;
                game.YellowUser = await _userRepository.FindByUserIdAsync(userId);
            }
            game.Status = GameStatus.InProgress;
            return new GameDetailsDTO(game);
        }

        public GameDetailsDTO Abandon(int userId, Guid gameId)
        {
            Game? game = _gameRepository.GetById(gameId);
            if (game is null)
            {
                throw new GameException("Partie introuvable");
            }
            if (game.RedUser?.Id == userId)
            {
                game.RedUserId = null;
                game.RedUser = null;
                game.RedIsConnected = false;
            }
            else if (game.YellowUser?.Id == userId)
            {
                game.YellowUserId = null;
                game.YellowUser = null;
                game.YellowIsConnected = false;
            }
            if (!game.RedIsConnected || !game.YellowIsConnected)
            {
                _gameRepository.Remove(game);
            }
            return new GameDetailsDTO(game);
        }

        public GameDetailsDTO Disconnect(int userId, Guid gameId)
        {
            Game? game = _gameRepository.GetById(gameId);
            if (game is null)
            {
                throw new GameException("Partie introuvable");
            }
            if (game.RedUser?.Id == userId)
            {
                game.RedIsConnected = false;
            }
            else if (game.YellowUser?.Id == userId)
            {
                game.YellowIsConnected = false;
            }
            if (!game.RedIsConnected && !game.YellowIsConnected)
            {
                _gameRepository.Remove(game);
            }
            return new GameDetailsDTO(game);
        }

        public GameDetailsDTO Reconnect(int userId, Guid gameId)
        {
            Game? game = _gameRepository.GetById(gameId);
            if (game is null)
            {
                throw new GameException("Partie introuvable");
            }
            if (game.RedUser?.Id == userId)
            {
                game.RedIsConnected = true;
            }
            else if (game.YellowUser?.Id == userId)
            {
                game.YellowIsConnected = true;
            }
            return new GameDetailsDTO(game);
        }

        private bool CheckWin(Color?[][] grid, int x, int y, Color color)
        {
            return CountDirection(grid, x, y, color, (x, y) => (x, y - 1)) + 1 >= 4
                || CountDirection(grid, x, y, color, (x, y) => (x - 1, y)) + 1 + CountDirection(grid, x, y, color, (x, y) => (x + 1, y)) >= 4
                || CountDirection(grid, x, y, color, (x, y) => (x - 1, y - 1)) + 1 + CountDirection(grid, x, y, color, (x, y) => (x + 1, y + 1)) >= 4
                || CountDirection(grid, x, y, color, (x, y) => (x - 1, y + 1)) + 1 + CountDirection(grid, x, y, color, (x, y) => (x + 1, y - 1)) >= 4;
        }

        private int CountDirection(Color?[][] grid, int x, int y, Color color, Func<int, int, (int,int)> direction)
        {
            (x, y) = direction(x, y);
            if(x < 0 || x >= grid.Length || y < 0 || y >= grid[0].Length)
            {
                return 0;
            }
            if (grid[x][y] != color)
            {
                return 0;
            }
            return 1 + CountDirection(grid, x, y, color, direction);
        }

        private bool IsFull(Color?[][] grid)
        {
            return grid.All(col => col.All(t => t != null));
        }
    }
}
