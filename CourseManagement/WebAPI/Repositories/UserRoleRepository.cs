using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ECourseContext _context;

        public UserRoleRepository(ECourseContext courseContext)
        {
            _context = courseContext;
        }
        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await _context.Set<UserRole>().ToListAsync();
        }

        public async Task<UserRole> GetByIdAsync(int id)
        {
            return await _context.Set<UserRole>().FindAsync(id);
        }


        public async Task<UserRole> AddAsync(UserRole userRole)
        {
            var result = await _context.Set<UserRole>().AddAsync(userRole);
            await _context.SaveChangesAsync();
            return result.Entity;
        }



        public async Task DeleteAsync(int id)
        {
            var userRole = await GetByIdAsync(id);
            if (userRole != null)
            {
                _context.Set<UserRole>().Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckUserRoleAsync(int id)
        {
            return await _context.Set<UserRole>().AnyAsync(ur => ur.UserRoleId == id);
        }

        
    }
}
