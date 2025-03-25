using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonVideoResponseAdmin> CreateLessonVideoAsync(LessonVideoRequestDto request);

        Task<LessonVideoResponseAdmin> UpdateLessonVideoAsync(int id, LessonVideoUpdateDto request);

        Task<LessonQuizzResponseAdmin> UpdateLessonQuizzAsync(int id, LessonQuizzUpdateDto request);

        Task<LessonQuizzResponseAdmin> CreateLessonQuizzAsync(LessonQuizzRequestDto request);
        Task<IEnumerable<LessonResponseAdmin>> GetAllLessonAsync();
        Task<LessonDetailResponseAdmin> GetLessonByIdAsync(int id);
    }
}
