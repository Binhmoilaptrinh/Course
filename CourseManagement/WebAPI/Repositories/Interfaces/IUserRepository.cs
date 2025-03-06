using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
<<<<<<< HEAD
        Task<List<UserRole>> GetAllAsync();
        Task<User> GetAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> CheckAsyncForUserName(string userName);
        Task<bool> CheckAsyncForEmail(string email);
        Task<User> GetUserAsyncForUserName(string userName);
        Task<User> GetUserAsyncForEmail(string email);

=======
        Task<User> GetAsync(int id); 
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
    }
}
