using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<AnswerResponse> CreateAnswerAsync(AnswerRequestDto request);

        Task<AnswerResponse> UpdateAnswerAsync(AnswerUpdateDto request);
    }
}
