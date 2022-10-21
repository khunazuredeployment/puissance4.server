using Puissance4.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puissance4.Business.Interfaces
{
    public interface IGameRepository
    {
        void Add(Game g);

        void Remove(Game g);

        Game? GetByPlayer(int id);

        IEnumerable<Game> GetAll();

        Game? GetById(Guid id);
    }
}
