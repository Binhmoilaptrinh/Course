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
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;

        public StaffController(IUserService userService, IStaffService staffService)
        {
            _userService = userService;
            _staffService = staffService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStaff()
        {
            var staffList = await _staffService.GetStaffReponses();
            return Ok(staffList);
        }

        [HttpPost("add")]
        
        public async Task<IActionResult> AddStaff([FromBody] StaffRequestDto staffDto)
        {
            if (staffDto == null)
            {
                return BadRequest("Staff data is required");
            }

            try
            {
                var result = await _staffService.AddStaff(staffDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStaff([FromBody] StaffReponseDto staffDto)
        {
            if (staffDto == null)
            {
                return BadRequest("Staff data is required");
            }

            try
            {
                var result = await _staffService.UpdateStaff(staffDto);
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
