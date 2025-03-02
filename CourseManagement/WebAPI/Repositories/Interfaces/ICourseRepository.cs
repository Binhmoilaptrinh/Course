using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();

        Task<Course> CreateAsync(Course course);
    }
}
