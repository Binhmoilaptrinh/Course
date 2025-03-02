using Microsoft.AspNetCore.Identity;
using WebAPI.DTOS;
using WebAPI.DTOS.Authentication;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthenticateService
    {

        Task<string> SignupAsync(SignupModel signup);

        Task<string> LoginAsync(LoginModel login);
    }
}
