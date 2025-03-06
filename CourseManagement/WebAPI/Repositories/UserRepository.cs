<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
=======
﻿using WebAPI.Models;
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
<<<<<<< HEAD
        private readonly ECourseContext _courseContext;

        public UserRepository(ECourseContext courseContext)
        {
            _courseContext = courseContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            _courseContext.Users.Add(user);
            await _courseContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _courseContext.Entry(user).State = EntityState.Modified;
            await _courseContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> CheckAsyncForUserName(string username)
        {
            var check = _courseContext.Users.Any(e => e.Username == username);
            await _courseContext.SaveChangesAsync();
            return check;

        }
        public async Task<bool> CheckAsyncForEmail(string email)
        {
            var check = _courseContext.Users.Any(e => e.Email == email);
            await _courseContext.SaveChangesAsync();
            return check;

        }
        
        public async Task<User> GetUserAsyncForUserName(string userName)
        {
            return await _courseContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        public async Task<User> GetUserAsyncForEmail(string email)
        {
            return await _courseContext.Users.FirstOrDefaultAsync(u => u.Email == email);
=======
        private readonly ECourseContext _context;


        public UserRepository(ECourseContext context)
        {
            _context = context;
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
        }

        public async Task<User> GetAsync(int id)
        {
<<<<<<< HEAD
            return await _courseContext.Users.FindAsync(id);
        }

        public async Task<List<UserRole>> GetAllAsync()
        {
            return await _courseContext.UserRoles
                .Include(ur => ur.Role)  // Lấy thông tin từ bảng Role
                .Include(ur => ur.User)  // Lấy thông tin từ bảng User
                .ToListAsync();
=======
            return await _context.Users.FindAsync(id);
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
        }
    }
}
