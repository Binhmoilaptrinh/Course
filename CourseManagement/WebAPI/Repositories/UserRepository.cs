using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECourseContext _context;

        public UserRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> CheckAsyncForUserName(string username)
        {
            return await _context.Users.AnyAsync(e => e.Username == username);
        }

        public async Task<bool> CheckAsyncForEmail(string email)
        {
            return await _context.Users.AnyAsync(e => e.Email == email);
        }

        public async Task<User> GetUserAsyncForUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task<User> GetUserAsyncForEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<UserRole>> GetAllAsync()
        {
            return await _context.UserRoles
                .Include(ur => ur.Role)  // Lấy thông tin từ bảng Role
                .Include(ur => ur.User)  // Lấy thông tin từ bảng User
                .ToListAsync();
        }

        public async Task<User?> GetUserByVerificationTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
        }


        public async Task<List<UserRole>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(u => u.UserId == userId) 
                .ToListAsync();
        }

        
    }
}
