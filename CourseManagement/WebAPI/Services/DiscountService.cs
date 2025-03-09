using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS.response;

namespace WebAPI.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<List<DiscountResponseDto>> GetAllAsync()
        {
            return await _discountRepository.GetAllAsync();
        }

        public async Task<DiscountResponseDto?> GetByIdAsync(int id)
        {
            return await _discountRepository.GetByIdAsync(id);
        }

        public async Task<DiscountResponseDto> CreateAsync(DiscountRequestDto dto)
        {
            return await _discountRepository.CreateAsync(dto);
        }

        public async Task<DiscountResponseDto?> UpdateAsync(int id, DiscountRequestDto dto)
        {
            return await _discountRepository.UpdateAsync(id, dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _discountRepository.DeleteAsync(id);
        }
    }
}
