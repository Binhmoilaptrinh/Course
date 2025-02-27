using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS;
using System;
using Microsoft.AspNetCore.Authorization;
using WebAPI.DTOS;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthenticationController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthentication loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isValid = await _authService.ValidateUser(loginModel);
            if (!isValid)
                return Unauthorized(new { message = "Invalid email or password." });

            var isConfirmed = await _authService.IsEmailConfirmed(loginModel.Email);
            if (!isConfirmed)
                return BadRequest(new { message = "Please confirm your email before logging in." });

            var token = await _authService.CreateToken();

            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });

            var roles = await _authService.GetUserRoles(loginModel.Email);
            var redirectUrl = roles.Contains("Administrator") ? "/admin/dashboard" : "/homepage/index";

            return Ok(new { token, redirectUrl, roles });
        }
    }
}
