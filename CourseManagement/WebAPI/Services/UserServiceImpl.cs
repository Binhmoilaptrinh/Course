using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return _userRepository.GetAsync(id);
        }
    }
}
