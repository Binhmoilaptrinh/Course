using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserRole>> GetAllAsync();
        Task<List<UserRole>> GetUserRolesAsync(int userId);
        Task<User> GetAsync(int id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> CheckAsyncForUserName(string userName);
        Task<bool> CheckAsyncForEmail(string email);
        Task<User> GetUserAsyncForUserName(string userName);
        Task<User> GetUserAsyncForEmail(string email);
        Task<User?> GetUserByVerificationTokenAsync(string token);

    }
}
