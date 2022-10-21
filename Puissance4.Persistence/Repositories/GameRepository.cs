using Puissance4.Business.Interfaces;
using Puissance4.Domain.Entities;

namespace Puissance4.Persistence.Repositories
{
    public class GameRepository: IGameRepository
    {
        private readonly ICollection<Game> _games = new List<Game>();

        public void Add(Game g)
        {
            _games.Add(g);
        }

        public void Remove(Game g)
        {
            _games.Remove(g);
        }

        public Game? GetByPlayer(int id)
        {
            return _games.FirstOrDefault(g => g.RedUserId == id || g.YellowUserId == id);
        }

        public IEnumerable<Game> GetAll()
        {
            return _games;
        }

        public Game? GetById(Guid id)
        {
            return _games.FirstOrDefault(g => g.Id == id);
        }

    }
}
