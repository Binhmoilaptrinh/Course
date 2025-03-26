using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionResponse> CreateQuestionAsync(QuestionRequestDto request);

        Task<QuestionResponse> UpdateQuestionAsync(QuestionUpdateDto request);
    }
}
