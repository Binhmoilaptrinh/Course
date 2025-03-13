using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUser()
        {
            var staffList = await _userService.GetUserReponses();
            return Ok(staffList);
        }

        [HttpPost("add")]
        
        public async Task<IActionResult> AddStaff([FromBody] UserRequestDto userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("User data is required");
            }

            try
            {
                var result = await _userService.AddUser(userDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserReponseDto userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Staff data is required");
            }

            try
            {
                var result = await _userService.UpdateUser(userDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            bool isVerified = await _userService.VerifyEmailAsync(token);

            if (!isVerified)
            {
                return BadRequest("Invalid or expired token.");
            }

            return Ok("Email verified successfully!");
        }

    }
}
