using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<UserRole> GetByIdAsync(int id);
        Task<UserRole> AddAsync(UserRole userRole);
        Task DeleteAsync(int id);
        Task<bool> CheckUserRoleAsync(int id);
    }

}
