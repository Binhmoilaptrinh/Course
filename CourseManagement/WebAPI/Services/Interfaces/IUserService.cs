using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<bool> VerifyEmailAsync(string token);

        Task<UserReponseDto> AddUser(UserRequestDto user);

        Task<UserReponseDto> UpdateUser(UserReponseDto user);

        Task<List<UserReponseDto>> GetUserReponses();
    }
}
