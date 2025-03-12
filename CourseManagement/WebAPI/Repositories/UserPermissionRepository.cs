using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly ECourseContext _context;

        public UserPermissionRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPermission>> GetAllAsync()
        {
            return await _context.Set<UserPermission>().ToListAsync();
        }

        public async Task<UserPermission> GetByIdAsync(int id)
        {
            return await _context.Set<UserPermission>().FindAsync(id);
        }

        public async Task AddAsync(UserPermission userPermission)
        {
            await _context.Set<UserPermission>().AddAsync(userPermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var userPermission = await GetByIdAsync(id);
            if (userPermission != null)
            {
                _context.Set<UserPermission>().Remove(userPermission);
                await _context.SaveChangesAsync();
            }
        }
    }

}
