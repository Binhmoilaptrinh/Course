using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface ICourseLearningService
    {
        Task<CourseLearningResponseDTO> GetCourseLearning(int courseId);
    }
}
