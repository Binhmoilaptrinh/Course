using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ECourseContext _context;

        public RolePermissionRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermission>> GetAllAsync()
        {
            return await _context.Set<RolePermission>().ToListAsync();
        }

        public async Task<RolePermission> GetByIdAsync(int id)
        {
            return await _context.Set<RolePermission>().FindAsync(id);
        }

        public async Task AddAsync(RolePermission rolePermission)
        {
            await _context.Set<RolePermission>().AddAsync(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rolePermission = await GetByIdAsync(id);
            if (rolePermission != null)
            {
                //_context.set<rolepermission>().remove(rolepermission);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<bool> CheckPermissionAsync(int roleId, string permissionName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .AnyAsync(rp => rp.RoleId == roleId && rp.Permission.PermissionName == permissionName);
        }

    }
}
