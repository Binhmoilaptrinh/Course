using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ECourseContext _courseContext;

        public UserRoleRepository(ECourseContext courseContext)
        {
            _courseContext = courseContext;
        }
        public async Task<UserRole> CreateAsync(UserRole userRole)
        {
            _courseContext.UserRoles.Add(userRole);
            await _courseContext.SaveChangesAsync();
            return userRole; 
        }

        public async Task<bool> CheckUserRoleAsync(int roleID)
        {
            var check = _courseContext.Roles.Any(e => e.RoleId == roleID);
            await _courseContext.SaveChangesAsync();
            return check;
        }

        public async Task<UserRole> UpdateAsync(UserRole userRole)
        {
            _courseContext.Entry(userRole).State = EntityState.Modified;
            await _courseContext.SaveChangesAsync();
            return userRole;

        }
    }
}
