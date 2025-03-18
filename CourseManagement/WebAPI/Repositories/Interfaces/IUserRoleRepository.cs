using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<LoginResponseDto> GetLoginUserById(int Id);
        Task<UserRole> GetByIdAsync(int id);
        Task<UserRole> AddAsync(UserRole userRole);
        Task DeleteAsync(int id);
        Task<bool> CheckUserRoleAsync(int roleID, int userID);
    }

}
