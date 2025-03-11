using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface ICourseService
    {
        IQueryable<CourseAdminResponseDto> GetAllCourse();

        Task<CourseAdminResponseDto> GetCourseByIdAsync(int id);
        Task<CourseAdminResponseDto> CreateCourseAsync(CourseRequestDto request);

        Task<CourseAdminResponseDto> UpdateCourseAsync(int id, CourseRequestDto course);

        Task DeleteAsync(int id);

        Task<bool> IsExistCourseByIdAsync(int id);
    }
}
