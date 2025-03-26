using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IChapterService
    {
        Task<ChapterResponse> CreateChapterAsync(ChapterRequestDto request);

        Task<ChapterResponse> UpdateChapterAsync(int id, ChapterRequestDto request);

        Task<IEnumerable<ChapterResponse>> GetAllChapterAsync();
        Task<ChapterResponse> GetChapterByIdAsync(int id);

        Task DeleteChapterById(int id);
    }
}
