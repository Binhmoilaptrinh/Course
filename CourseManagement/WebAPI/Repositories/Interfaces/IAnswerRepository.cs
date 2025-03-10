using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> AddAnswerAsync(Answer answer);
    }
}
