using System.Security.Cryptography;
using AutoMapper;
using Azure.Storage.Blobs;
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
        private BlobContainerClient _filesContainer;

        public UserServiceImpl(IUserRepository userRepository, IUserRoleRepository repoUR, IMapper mapper, ISendEmail emailSender, IAuthenticateService authenticateService, IConfiguration configuration, BlobContainerClient filesContainer)
        {
            _userRepository = userRepository;
            _repoUR = repoUR;
            _mapper = mapper;
            _emailSender = emailSender;
            _authenticateService = authenticateService;
            _configuration = configuration;
            _filesContainer = filesContainer;
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

        public async  Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return null;
            }

            return user;
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
            newUser.IsEmailVerify = false;

            // 🔥 Tạo token xác minh email
            string token = GenerateVerificationToken();
            DateTime expiry = DateTime.UtcNow.AddHours(24);
            newUser.EmailVerificationToken = token;
            newUser.EmailVerificationExpiry = expiry;

            // 🔥 Upload ảnh Bio lên Azure Blob Storage
            if (user.Avatar != null)
            {
                string fileName = $"bio_{DateTime.UtcNow.Ticks}_{user.Avatar.FileName}";
                BlobClient client = _filesContainer.GetBlobClient(fileName);

                await using (Stream data = user.Avatar.OpenReadStream())
                {
                    await client.UploadAsync(data, overwrite: true);
                }

                // Lưu URL của ảnh vào User
                newUser.Avatar = client.Uri.AbsoluteUri;
            }

            var createdUser = await _userRepository.CreateAsync(newUser);
            if (createdUser != null)
            {
                string baseUrl = _configuration["AppSettings:BaseUrl"];
                string verificationLink = $"{baseUrl}/api/user/verify-email?token={token}";

                string emailBody = $"Here is your new password: {password} <br/>" +
                                   $"Click <a href='{verificationLink}'>here</a> to verify your email.";

                await _emailSender.SendEmailAsync(createdUser.Email, "Account Created - Verify Your Email", emailBody);

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


        public async Task<UserReponseDto> UpdateUser(int id, UserRequestDto user)
        {
            var existingUser = await _userRepository.GetAsync(id);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {id} not found");
            }

            var checkU = await _userRepository.GetUserAsyncForUserName(user.UserName);
            if (checkU != null && checkU.UserId != id)
            {
                throw new Exception($"User with {user.UserName} is existed");
            }

            existingUser.Username = user.UserName;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Bio = user.Bio;

            // 🔥 Kiểm tra nếu có ảnh mới thì upload, nếu không thì giữ nguyên
            if (user.Avatar != null)
            {
                string fileName = $"bio_{DateTime.UtcNow.Ticks}_{user.Avatar.FileName}";
                BlobClient client = _filesContainer.GetBlobClient(fileName);

                await using (Stream data = user.Avatar.OpenReadStream())
                {
                    await client.UploadAsync(data, overwrite: true);
                }

                existingUser.Avatar = client.Uri.AbsoluteUri;
            }

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            var userResponse = _mapper.Map<UserReponseDto>(updatedUser);
            userResponse.SelectedRole = user.SelectedRole;
            return userResponse;
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
                userReponse.Avatar = e.User.Avatar;
                userReponse.Bio = e.User.Bio;
                userReponse.PhoneNumber = e.User.PhoneNumber;
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
