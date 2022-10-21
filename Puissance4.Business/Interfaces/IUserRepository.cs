using Puissance4.Domain.Entities;
namespace Puissance4.Business.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindByUserIdAsync(int id);
        Task<User?> FindByUsernameAsync(string username);
        Task<bool> AnyByUsernameAsync(string username);
        Task<int> AddAsync(User user);
    }
}
