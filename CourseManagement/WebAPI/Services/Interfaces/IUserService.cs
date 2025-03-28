using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<UserReponseDto> GetUserResponseByIdAsync(int id);
        
        Task<UserReponseDto> AddUser(UserRequestDto user);

        Task<UserReponseDto> UpdateUser(int id, UserRequestDto user);

        Task<List<UserReponseDto>> GetUserReponses();
        Task<bool> VerifyEmailAsync(string token);
    }
}
