using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonVideoResponseAdmin> CreateLessonVideoAsync(LessonVideoRequestDto request);

        Task<LessonQuizzResponseAdmin> CreateLessonQuizzAsync(LessonQuizzRequestDto request);
        Task<IEnumerable<LessonResponseAdmin>> GetAllLessonAsync();
        Task<LessonResponseAdmin> GetLessonByIdAsync(int id);
    }
}
