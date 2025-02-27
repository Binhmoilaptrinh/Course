using Microsoft.AspNetCore.Identity;
using WebAPI.DTOS;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthenticateService
    {
        //Task<IdentityResult> RegisterUser(UserForRegistration userForRegistration);
        Task<bool> ValidateUser(UserForAuthentication userForAuth);
        Task<string> CreateToken();
        Task<bool> IsEmailConfirmed(string email);
        Task<IList<string>> GetUserRoles(string email);
    }
}
