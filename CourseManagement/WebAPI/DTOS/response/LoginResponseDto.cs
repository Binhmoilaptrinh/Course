using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOS.response
{
    public class LoginResponseDto
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public bool Success { get; set; }  // ✅ Thêm thuộc tính Success
        public string? Token { get; set; }
        public string Message { get; set; }
    }
}
