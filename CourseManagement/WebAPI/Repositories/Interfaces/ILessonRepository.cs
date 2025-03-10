using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson> CreateAsync(Lesson lesson);

        Task<IEnumerable<Lesson>> GetAllAsync();

        Task<Lesson> GetByIdAsync(int id);
    }
}
