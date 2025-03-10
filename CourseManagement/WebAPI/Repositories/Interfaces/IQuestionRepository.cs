using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question question);
    }
}
