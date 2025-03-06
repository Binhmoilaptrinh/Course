using WebAPI.DTOS.reponse;
using WebAPI.DTOS.request;

namespace WebAPI.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountResponseDto>> GetAllAsync();
        Task<DiscountResponseDto?> GetByIdAsync(int id);
        Task<DiscountResponseDto> CreateAsync(DiscountRequestDto dto);
        Task<DiscountResponseDto?> UpdateAsync(int id, DiscountRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
