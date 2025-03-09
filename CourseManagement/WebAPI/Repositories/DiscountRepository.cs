using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.request;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.DTOS.response;

namespace WebAPI.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ECourseContext _context;
        private readonly IMapper _mapper;

        public DiscountRepository(ECourseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Lấy tất cả Discount (bao gồm thông tin Course)
        public async Task<List<DiscountResponseDto>> GetAllAsync()
        {
            var discounts = await _context.Discounts
                .Include(d => d.Course) // Include để lấy thông tin Course
                .Select(d => new DiscountResponseDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    DiscountPer = d.DiscountPer,
                    MaxUses = d.MaxUses,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    CreatedAt = d.CreatedAt,
                    CourseID = d.CourseId,
                    CourseName = d.Course != null ? d.Course.Title : null // Chỉ lấy CourseName
                })
                .ToListAsync();

            return _mapper.Map<List<DiscountResponseDto>>(discounts);
        }

        // Lấy Discount theo ID
        public async Task<DiscountResponseDto?> GetByIdAsync(int id)
        {
            var discount = await _context.Discounts
                .Include(d => d.Course)
                .Select(d => new DiscountResponseDto
                {
                    Id = d.Id,
                    Code = d.Code,
                    DiscountPer = d.DiscountPer,
                    MaxUses = d.MaxUses,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    CreatedAt = d.CreatedAt,
                    CourseID = d.CourseId,
                    CourseName = d.Course != null ? d.Course.Title : null // Chỉ lấy CourseName
                })
                .FirstOrDefaultAsync(d => d.Id == id);

            return _mapper.Map<DiscountResponseDto>(discount);
        }

        public async Task<DiscountResponseDto> CreateAsync(DiscountRequestDto dto)
        {
            // Kiểm tra xem CourseId có hợp lệ không
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!courseExists)
            {
                throw new Exception("Invalid CourseId: Course does not exist.");
            }

            // Ánh xạ DTO sang Entity
            var discount = _mapper.Map<Discount>(dto);
            if (discount == null)
            {
                throw new Exception("Mapping failed: discount is null");
            }

            // Sinh mã giảm giá và set ngày tạo
            discount.Code = "DIS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            discount.CreatedAt = DateTime.Now;

            try
            {
                _context.Discounts.Add(discount);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message);
            }

            return _mapper.Map<DiscountResponseDto>(discount);
        }


        // Cập nhật Discount
        public async Task<DiscountResponseDto?> UpdateAsync(int id, DiscountRequestDto dto)
        {
            var existingDiscount = await _context.Discounts.FindAsync(id);
            if (existingDiscount == null) return null;

            _mapper.Map(dto, existingDiscount);
            existingDiscount.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return _mapper.Map<DiscountResponseDto>(existingDiscount);
        }

        // Xóa Discount
        public async Task<bool> DeleteAsync(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) return false;

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
