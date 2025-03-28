using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using Microsoft.EntityFrameworkCore;
using WebAPI.Filters;
using WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.OData.Query;
using Azure.Storage.Blobs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, BlobContainerClient filesContainer)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        [EnableQuery] // Cho phép sử dụng OData query
        public ActionResult<IQueryable<UserReponseDto>> GetUsers()
        {
            var users = _userService.GetUserReponses().Result.AsQueryable();
            return Ok(users);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromForm] UserRequestDto userDTO)
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


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserRequestDto userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("User data is required");
            }

            try
            {
                var result = await _userService.UpdateUser(id, userDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var userResponse = await _userService.GetUserResponseByIdAsync(id);

            if (userResponse == null)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }

            return Ok(userResponse);
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var userResponse = await _userService.GetUserResponseByIdAsync(int.Parse(userIdClaim));
            if (userResponse == null) return NotFound("User not found");

            return Ok(userResponse);
        }


        [HttpGet("current-user")]
        public IActionResult GetCurrentUserId()
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User is not logged in");

            return Ok(new { userId = int.Parse(userId) });
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
