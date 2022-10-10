using System.Data;

namespace Puissance4.Persistence.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly IDbConnection _connection;

        protected RepositoryBase(IDbConnection connection)
        {
            _connection = connection;
        }
    }
}
