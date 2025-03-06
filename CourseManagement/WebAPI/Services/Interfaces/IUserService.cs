using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
    }
}
