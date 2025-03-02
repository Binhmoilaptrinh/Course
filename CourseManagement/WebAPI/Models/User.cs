namespace WebAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }



        // Thêm các trường khác nếu cần
    }
}
