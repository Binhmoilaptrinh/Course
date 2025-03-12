namespace WebAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public bool IsEmailVerify { get; set; } // Thêm xác minh Email

        // Thêm mới
        public string? EmailVerificationToken { get; set; } // Token xác minh email
        public DateTime? EmailVerificationExpiry { get; set; } // Thời gian hết hạn token
        // Thêm các trường khác nếu cần
    }
}
