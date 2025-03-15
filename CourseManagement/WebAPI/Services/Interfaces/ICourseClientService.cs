using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface ICourseClientService
    {
        //public Task<IEnumerable<CourseClientDTO>> GetCourseListHomePageFree123();

        public Task<IEnumerable<CourseClientDTO>> GetCourseListHomePageFree();

        public Task<IEnumerable<CourseClientDTO>> GetProCourses();

        public Task<CourseDetailResponse> GetCourseDetailHomepage(int id);
    }
}
