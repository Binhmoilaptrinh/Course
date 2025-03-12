using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ECourseContext _context;

        public PermissionRepository(ECourseContext eCourseContext)
        {
            _context = eCourseContext;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Set<Permission>().ToListAsync();
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await _context.Set<Permission>().FindAsync(id);
        }

        public async Task AddAsync(Permission permission)
        {
            await _context.Set<Permission>().AddAsync(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            _context.Set<Permission>().Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = await GetByIdAsync(id);
            if (permission != null)
            {
                _context.Set<Permission>().Remove(permission);
                await _context.SaveChangesAsync();
            }
        }
    }
}
