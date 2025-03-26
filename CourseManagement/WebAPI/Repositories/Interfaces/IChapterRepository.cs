using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IChapterRepository
    {
        Task<Chapter> CreateAsync(Chapter chapter);
        Task<Chapter> UpdateAsync(Chapter chapter);

        Task<IEnumerable<Chapter>> GetAllAsync();
        Task<Chapter> GetByIdAsync(int id);

        Task DeleteAsync(int id);
    }
}
