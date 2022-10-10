using Dapper;
using Puissance4.Business.Interfaces;
using Puissance4.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puissance4.Persistence.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public Task<int> AddAsync(User user)
        {
            return _connection.ExecuteScalarAsync<int>(
                "INSERT INTO [user](Username, Password, Salt) VALUES (@Username, @Password, @Salt)", 
                user
            );
        }

        public Task<bool> AnyByUsernameAsync(string username)
        {
            return _connection.ExecuteScalarAsync<bool>(
                "SELECT COUNT(Id) FROM [User] WHERE username = @Username",
                new { Username = username }
            );
        }

        public Task<User?> FindByUsernameAsync(string username)
        {
            return _connection.QueryFirstOrDefaultAsync<User?>(
                "SELECT * FROM [User] WHERE username = @Username", 
                new { Username = username }
            );
        }
    }
}
