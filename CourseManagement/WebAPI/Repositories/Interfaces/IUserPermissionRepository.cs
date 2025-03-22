using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserPermissionRepository
    {
        Task<IEnumerable<UserPermission>> GetAllAsync();
        Task<UserPermission> GetByIdAsync(int id);
        Task AddAsync(UserPermission userPermission);
        Task DeleteAsync(int id);
    }

}
