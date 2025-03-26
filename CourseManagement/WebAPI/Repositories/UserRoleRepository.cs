using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.response;
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

        public async Task<bool> CheckUserRoleAsync(int roleId, int userID)
        {
            return await _context.Set<UserRole>().AnyAsync(ur => ur.RoleId == roleId && ur.UserId == userID);
        }

        public async Task<LoginResponseDto?> GetLoginUserById(int Id)
        {
            var u = await _context.Set<UserRole>()
                                  .Include(ur => ur.Role)
                                  .Include(ur => ur.User)
                                  .FirstOrDefaultAsync(ur => ur.UserId == Id);

            // Kiểm tra nếu u là null
            if (u == null)
            {
                return null; // hoặc throw Exception nếu muốn xử lý lỗi
            }

            // Khởi tạo đúng cách
            LoginResponseDto loginDto = new LoginResponseDto
            {
                UserId = u.UserId,
                UserName = u.User.Username,
                Email = u.User.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.RoleName,
                Avatar = u.User.Avatar,
                Bio = u.User.Bio,
                PhoneNumber = u.User.PhoneNumber
            };

            return loginDto;
        }

    }
}
