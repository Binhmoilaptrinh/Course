using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface ICourseLearningService
    {
        Task<CourseLearningResponseDTO> GetCourseLearning(int courseId);
        Task<LessonProgressResponse> GetLessonProgress(int lessonId, int userId);
        Task<LessonProgress> EnrollLesson(LessonEnroll enroll);
        Task<LessonProgress> UpdateProgressLesson(ProgressLessonUpdate progress);
        Task<int> GetLatestLesson(LessonProgressLatest lessonProgress);
    }
}
