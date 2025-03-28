using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOS.response
{
    public class UserReponseDto
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        public int SelectedRole { get; set; }

        public string? RoleName { get; set; }
        public string? Avatar { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
    }
}
