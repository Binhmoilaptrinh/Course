using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DTOS.Authentication;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ECourseContext _eCourseContext;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IConfiguration _configuration;
        public readonly ISendEmail _emailSender;

        public AuthenticateService(ECourseContext eCourseContext, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IConfiguration configuration, ISendEmail emailSender)
        {
            _eCourseContext = eCourseContext;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var user = await _userRepository.GetUserAsyncForUserName(login.Username);

            if (user == null || !await VerifyPasswordAsync(login.Password, user.Password))
            {
                return null; // Trả về null nếu username không tồn tại hoặc mật khẩu không khớp
            }

            if (!user.IsEmailVerify)
            {
                return null;
            }

            var userRoles = await _eCourseContext.UserRoles
                .Where(x => x.UserId == user.UserId)
                .Select(x => x.Role.RoleName)
                .ToListAsync();
            

            string secret = _configuration["JwtSettings:secretKey"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // ✅ Thêm UserId
        new Claim(JwtRegisteredClaimNames.Sub, login.Username),
    };

            // ✅ Thêm nhiều Roles vào Claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // ✅ Tăng thời gian hết hạn
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JwtSettings:validIssuer"],
                Audience = _configuration["JwtSettings:validAudience"]
            };
            LoginResponseDto loginResponse = await _userRoleRepository.GetLoginUserById(user.UserId);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            loginResponse.Token = tokenHandler.WriteToken(token);
            loginResponse.Success = true;

            return loginResponse;
        }

    //    public async Task<string> LoginAsync(LoginModel login)
    //    {
    //        if (login == null)
    //        {
    //            throw new ArgumentNullException(nameof(login));
    //        }

    //        var user = await _eCourseContext.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

    //        if (user == null || !await VerifyPasswordAsync(login.Password, user.Password))
    //        {
    //            return null; // Trả về null nếu username không tồn tại hoặc mật khẩu không khớp
    //        }

    //        if (!user.IsEmailVerify)
    //        {
    //            return "Email is not verified to login";
    //        }

    //        var userRoles = await _eCourseContext.UserRoles
    //            .Where(x => x.UserId == user.UserId)
    //            .Select(x => x.Role.RoleName)
    //            .ToListAsync();

    //        string secret = _configuration["JwtSettings:secretKey"];
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = Encoding.UTF8.GetBytes(secret);

    //        var claims = new List<Claim>
    //{
    //    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // ✅ Thêm UserId
    //    new Claim(JwtRegisteredClaimNames.Sub, login.Username),
    //};

    //        // ✅ Thêm nhiều Roles vào Claims
    //        foreach (var role in userRoles)
    //        {
    //            claims.Add(new Claim(ClaimTypes.Role, role));
    //        }

    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(claims),
    //            Expires = DateTime.UtcNow.AddMinutes(30), // ✅ Tăng thời gian hết hạn
    //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
    //            Issuer = _configuration["JwtSettings:validIssuer"],
    //            Audience = _configuration["JwtSettings:validAudience"]
    //        };

    //        var token = tokenHandler.CreateToken(tokenDescriptor);
    //        return tokenHandler.WriteToken(token);
    //    }



        public async Task<string> SignupAsync(SignupModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return "Username and Password are required.";
            }

            var existingUser = await _eCourseContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                return "Username already exists.";
            }

            // Hash password trước khi lưu vào database
            string hashedPassword = await HashPasswordAsync(user.Password);

            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword
            };
            newUser.IsEmailVerify = false; // Chưa xác minh email
            // 🔥 Tạo token xác minh email
            string token = GenerateVerificationToken();
            DateTime expiry = DateTime.UtcNow.AddHours(24); // Token hết hạn sau 24 giờ

            newUser.EmailVerificationToken = token;
            newUser.EmailVerificationExpiry = expiry;

            _eCourseContext.Users.Add(newUser);
            await _eCourseContext.SaveChangesAsync();

            string baseUrl = _configuration["AppSettings:BaseUrl"];
            string verificationLink = $"{baseUrl}/api/staff/verify-email?token={token}";

            string emailBody = $"Click <a href='{verificationLink}'>here</a> to verify your email.";

            await _emailSender.SendEmailAsync(user.Email, "Account Created - Verify Your Email", emailBody);

            var savedUser = await _eCourseContext.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            if (savedUser == null)
            {
                return "User creation failed.";
            }

            //// Kiểm tra role hợp lệ
            //var roleExists = await _eCourseContext.Roles.AnyAsync(r => r.RoleId == user.RoleId);
            //if (!roleExists)
            //{
            //    return "Invalid role.";
            //}

            _eCourseContext.UserRoles.Add(new UserRole { UserId = savedUser.UserId, RoleId = 1002 });
            await _eCourseContext.SaveChangesAsync();

            return "User registered successfully.";
        }

        private async Task<bool> VerifyPasswordAsync(string inputPassword, string storedHashedPassword)
        {
            string hashedInput = await HashPasswordAsync(inputPassword); // Hash lại mật khẩu nhập vào
            return hashedInput == storedHashedPassword; // So sánh với mật khẩu đã lưu
        }

        public Task<string> HashPasswordAsync(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return Task.FromResult(builder.ToString());
            }
        }

        private string GenerateVerificationToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); // Token 32 bytes
        }

        
    }
}
