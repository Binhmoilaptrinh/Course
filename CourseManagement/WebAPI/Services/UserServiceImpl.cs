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

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetUserByVerificationTokenAsync(token);

            if (user == null || user.EmailVerificationExpiry < DateTime.UtcNow)
            {
                return false; // Token không hợp lệ hoặc hết hạn
            }

            // Cập nhật trạng thái xác minh email
            user.IsEmailVerify = true;
            user.EmailVerificationToken = null; // Xóa token
            user.EmailVerificationExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
