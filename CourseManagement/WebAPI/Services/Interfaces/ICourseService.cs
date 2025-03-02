using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseAdminResponseDto>> GetAllCourseAsync();

        Task<CourseAdminResponseDto> CreateCourseAsync(CourseCreateDto request);
    }
}
