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

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
