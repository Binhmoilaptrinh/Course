using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> CreateAsync(UserRole userRole);
        Task<UserRole> UpdateAsync(UserRole userRole);
        Task<bool> CheckUserRoleAsync(int roleID);
    }
}
