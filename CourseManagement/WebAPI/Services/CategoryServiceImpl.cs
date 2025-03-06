using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryServiceImpl(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequestDto requestDto)
        {
            var category = _mapper.Map<Category>(requestDto);
            var createdCate = await _repository.CreateAsync(category);
            return _mapper.Map<CategoryResponse>(createdCate);
        }

        public async Task DeleteCategoryAsyc(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with Id {id} not found");
            }
            await _repository.DeleteAsyc(id);
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception($"Category with Id {id} not found");
            }
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryRequestDto requestDto)
        {
            var existingCate = await _repository.GetByIdAsync(id);
            if (existingCate == null)
            {
                throw new Exception($"Category with Id {id} not found");
            }
            _mapper.Map(requestDto, existingCate);
            var updateCate = await _repository.UpdateAsync(existingCate);
            return _mapper.Map<CategoryResponse>(updateCate);
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
            return await _repository.IsExistByIdAsync(id);
        }
    }
}
