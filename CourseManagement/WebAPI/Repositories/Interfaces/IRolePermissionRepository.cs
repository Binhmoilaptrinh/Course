using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IRolePermissionRepository 
    {
        Task<IEnumerable<RolePermission>> GetAllAsync();
        Task<RolePermission> GetByIdAsync(int id);
        Task AddAsync(RolePermission rolePermission);
        Task DeleteAsync(int id);
        Task<bool> CheckPermissionAsync(int roleId, string permissionName);
    }
}
