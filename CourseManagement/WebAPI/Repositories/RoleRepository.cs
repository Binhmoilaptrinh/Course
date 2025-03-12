using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ECourseContext _context;

        public RoleRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Set<Role>().ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Set<Role>().FindAsync(id);
        }

        public async Task AddAsync(Role role)
        {
            await _context.Set<Role>().AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Set<Role>().Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role != null)
            {
                _context.Set<Role>().Remove(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
