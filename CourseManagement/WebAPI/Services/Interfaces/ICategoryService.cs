using WebAPI.DTOS.request;
using WebAPI.DTOS.response;

namespace WebAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync();
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequestDto category);

        Task<CategoryResponse> GetCategoryByIdAsync(int id);

        Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryRequestDto category);

        Task DeleteCategoryAsyc(int id);

        Task<bool> IsExistByIdAsync(int id);
    }
}
