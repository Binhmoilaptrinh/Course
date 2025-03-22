using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS;
using System;
using Microsoft.AspNetCore.Authorization;
using WebAPI.DTOS;
using WebAPI.DTOS.Authentication;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

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

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupModel user)
        {
            try
            {
                var result = await _authService.SignupAsync(user);
                if (result == "User registered successfully.")
                {
                    return Ok(new { message = result });
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                var response = await _authService.LoginAsync(login);

                if (response == null || !response.Success)
                {
                    return Unauthorized(new { message = response?.Message ?? "Invalid credentials" });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }




    }
}
