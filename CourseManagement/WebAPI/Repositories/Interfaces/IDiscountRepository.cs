using WebAPI.DTOS.request;
using WebAPI.DTOS.reponse;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<List<DiscountResponseDto>> GetAllAsync();
        Task<DiscountResponseDto?> GetByIdAsync(int id);
        Task<DiscountResponseDto> CreateAsync(DiscountRequestDto dto);
        Task<DiscountResponseDto?> UpdateAsync(int id, DiscountRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
