using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question question);

        Task<Question> UpdateQuestionAsync(Question question);

        Task<Question> GetQuestionByIdAsync(int id);
    }
}
