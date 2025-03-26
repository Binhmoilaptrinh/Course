using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> AddAnswerAsync(Answer answer);

        Task<Answer> UpdateAnswerAsync(Answer answer);

        Task<Answer> GetAnswerByIdAsync(int id);
    }
}
