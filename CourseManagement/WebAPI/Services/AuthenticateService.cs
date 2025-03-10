using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.DTOS.Authentication;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly ECourseContext _eCourseContext;
        private readonly IConfiguration _configuration;

        public AuthenticateService(ECourseContext eCourseContext, IConfiguration configuration)
        {
            _eCourseContext = eCourseContext;
            _configuration = configuration;
        }


        public async Task<string> LoginAsync(LoginModel login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var user = await _eCourseContext.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            if (user == null || !await VerifyPasswordAsync(login.Password, user.Password))
            {
                return null; // Trả về null nếu username không tồn tại hoặc mật khẩu không khớp
            }

            var userRole = await _eCourseContext.UserRoles
                .Where(x => x.UserId == user.UserId)
                .Select(x => x.Role.RoleName)
                .FirstOrDefaultAsync();

            string secret = _configuration["JwtSettings:secretKey"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, login.Username),
        new Claim(ClaimTypes.Role, userRole)
    }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _configuration["JwtSettings:validIssuer"],
                Audience = _configuration["JwtSettings:validAudience"]
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


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

            _eCourseContext.Users.Add(newUser);
            await _eCourseContext.SaveChangesAsync();

            var savedUser = await _eCourseContext.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            if (savedUser == null)
            {
                return "User creation failed.";
            }

            // Kiểm tra role hợp lệ
            var roleExists = await _eCourseContext.Roles.AnyAsync(r => r.RoleId == user.RoleId);
            if (!roleExists)
            {
                return "Invalid role.";
            }

            _eCourseContext.UserRoles.Add(new UserRole { UserId = savedUser.UserId, RoleId = user.RoleId });
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

    }
}
