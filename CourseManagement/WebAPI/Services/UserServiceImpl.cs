using System.Security.Cryptography;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;
        public readonly IUserRoleRepository _repoUR;
        public readonly IMapper _mapper;
        public readonly ISendEmail _emailSender;
        public readonly IAuthenticateService _authenticateService;
        private readonly IConfiguration _configuration;

        public UserServiceImpl(IUserRepository userRepository, IUserRoleRepository repoUR, IMapper mapper, ISendEmail emailSender, IAuthenticateService authenticateService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _repoUR = repoUR;
            _mapper = mapper;
            _emailSender = emailSender;
            _authenticateService = authenticateService;
            _configuration = configuration;
        }

        public async Task<UserReponseDto> GetUserResponseByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
            {
                return null;
            }

            // Ánh xạ entity User sang DTO
            var userResponse = _mapper.Map<UserReponseDto>(user);
            return userResponse;
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<UserReponseDto> AddUser(UserRequestDto user)
        {
            var newUser = _mapper.Map<User>(user);

            // Kiểm tra username & email đã tồn tại chưa
            if (await _userRepository.CheckAsyncForUserName(user.UserName))
                throw new Exception($"User with {user.UserName} is existed");
            if (await _userRepository.CheckAsyncForEmail(user.Email))
                throw new Exception($"User with {user.Email} is existed");

            var password = GenerateRandomPassword();
            newUser.Password = await _authenticateService.HashPasswordAsync(password);
            newUser.IsEmailVerify = false; // Chưa xác minh email

            // 🔥 Tạo token xác minh email
            string token = GenerateVerificationToken();
            DateTime expiry = DateTime.UtcNow.AddHours(24); // Token hết hạn sau 24 giờ

            newUser.EmailVerificationToken = token;
            newUser.EmailVerificationExpiry = expiry;

            var createdUser = await _userRepository.CreateAsync(newUser);
            if (createdUser != null)
            {
                string baseUrl = _configuration["AppSettings:BaseUrl"];
                string verificationLink = $"{baseUrl}/api/user/verify-email?token={token}";

                string emailBody = $"Here is your new password: {password} <br/>" +
                                   $"Click <a href='{verificationLink}'>here</a> to verify your email.";

                await _emailSender.SendEmailAsync(createdUser.Email, "Account Created - Verify Your Email", emailBody);

                // Kiểm tra role hợp lệ
                if (await _repoUR.CheckUserRoleAsync(user.SelectedRole, createdUser.UserId))
                    throw new Exception($" {user.SelectedRole} is invalid");

                var userRole = await _repoUR.AddAsync(new UserRole { UserId = createdUser.UserId, RoleId = user.SelectedRole });
                if (userRole == null)
                    throw new Exception($" {user.SelectedRole} is invalid");
            }

            var userResponse = _mapper.Map<UserReponseDto>(createdUser);
            userResponse.SelectedRole = user.SelectedRole;
            return userResponse;
        }

        public async Task<UserReponseDto> UpdateUser(UserReponseDto user)
        {
            var existingUser = await _userRepository.GetAsync(user.UserId); // Lấy thông tin user hiện tại
            if (existingUser == null)
            {
                throw new Exception($"User with ID {user.UserId} not found");
            }

            var checkU = await _userRepository.GetUserAsyncForUserName(user.UserName);
            if (checkU != null && checkU.UserId != user.UserId)  // Kiểm tra username có bị trùng với user khác không
            {
                throw new Exception($"User with {user.UserName} is existed");
            }

            var checkE = await _userRepository.GetUserAsyncForEmail(user.Email);
            if (checkE != null && checkE.UserId != user.UserId)  // Kiểm tra email có bị trùng với user khác không
            {
                throw new Exception($"User with {user.Email} is existed");
            }

            // Cập nhật thông tin user
            existingUser.Username = user.UserName;
            existingUser.Email = user.Email;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            //if (updatedUser != null)
            //{
            //    // Kiểm tra role hợp lệ
            //    var roleExists = await _repoUR.CheckUserRoleAsync(staff.SelectedRole);
            //    if (!roleExists)
            //    {
            //        throw new Exception($"{staff.SelectedRole} is invalid");
            //    }

            //    var userRole = await _repoUR.UpdateAsync(new UserRole { UserId = updatedUser.UserId, RoleId = staff.SelectedRole });
            //    if (userRole == null)
            //    {
            //        throw new Exception($"{staff.SelectedRole} is invalid");
            //    }
            //}

            var staffResponse = _mapper.Map<UserReponseDto>(updatedUser);
            staffResponse.SelectedRole = user.SelectedRole;
            return staffResponse;
        }

        public async Task<List<UserReponseDto>> GetUserReponses()
        {
            var userWRole = await _userRepository.GetAllAsync();
            List<UserReponseDto> list = new List<UserReponseDto>();

            foreach (var e in userWRole)
            {
                UserReponseDto userReponse = new UserReponseDto();
                userReponse.UserId = e.UserId;
                userReponse.UserName = e.User.Username;
                userReponse.Email = e.User.Email;
                userReponse.SelectedRole = e.RoleId;
                userReponse.RoleName = e.Role.RoleName;
                list.Add(userReponse);
            }

            return list;

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


        public static string GenerateRandomPassword(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

        private string GenerateVerificationToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
            return Base64UrlEncoder.Encode(randomBytes);
        }

        
    }
}
