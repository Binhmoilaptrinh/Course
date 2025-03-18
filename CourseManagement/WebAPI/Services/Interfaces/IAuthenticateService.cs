using Microsoft.AspNetCore.Identity;
using WebAPI.DTOS;
using WebAPI.DTOS.Authentication;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthenticateService
    {

        Task<string> SignupAsync(SignupModel signup);

        //Task<string> LoginAsync(LoginModel login);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto login);
        Task<string> HashPasswordAsync(string password);

        
    }
}
