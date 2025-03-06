using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int id); 
    }
}
