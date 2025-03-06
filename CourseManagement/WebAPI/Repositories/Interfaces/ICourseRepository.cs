using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);

        Task<bool> IsExistByIdAsync(int id);

        Task<Course> CreateAsync(Course course);

        Task<Course> UpdateAsync(Course course);

        Task DeleteAsync(int id);

    }
}
