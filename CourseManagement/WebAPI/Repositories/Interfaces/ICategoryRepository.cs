using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category category);

        Task<Category> GetByIdAsync(int id);

        Task<Category> UpdateAsync(Category category);

        Task DeleteAsyc(int id);

        Task<bool> IsExistByIdAsync(int id);
    }
}
