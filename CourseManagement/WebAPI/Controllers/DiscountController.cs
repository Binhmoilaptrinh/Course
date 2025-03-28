using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.Services.Interfaces;
using WebAPI.DTOS.response;
using WebAPI.Filters;
using Microsoft.AspNetCore.OData.Query;

namespace WebAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // Lấy danh sách Discount
        [HttpGet("all")]
        [EnableQuery]
        //[AuthorizePermission("ManageDiscount")]
        public async Task<ActionResult<List<DiscountResponseDto>>> GetAll()
        {
            var discounts = await _discountService.GetAllAsync();
            return discounts.Any() ? Ok(discounts) : NoContent();
        }

        // Lấy Discount theo ID
        [HttpGet("{id}")]
        //[AuthorizePermission("ManageDiscount")]

        public async Task<ActionResult<DiscountResponseDto>> GetById(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);
            return discount != null ? Ok(discount) : NotFound(new { message = "Discount not found" });
        }

        // Tạo mới Discount
        [HttpPost]
        //[AuthorizePermission("ManageDiscount")]

        public async Task<ActionResult<DiscountResponseDto>> Create([FromBody] DiscountRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDiscount = await _discountService.CreateAsync(dto);
            return createdDiscount != null
                ? CreatedAtAction(nameof(GetById), new { id = createdDiscount.Id }, createdDiscount)
                : BadRequest(new { message = "Failed to create discount" });
        }

        // Cập nhật Discount
        [HttpPut("{id}")]
        //[AuthorizePermission("ManageDiscount")]

        public async Task<ActionResult<DiscountResponseDto>> Update(int id, [FromBody] DiscountRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDiscount = await _discountService.UpdateAsync(id, dto);
            return updatedDiscount != null ? Ok(updatedDiscount) : NotFound(new { message = "Discount not found" });
        }

        // Xóa Discount
        [HttpDelete("{id}")]
        //[AuthorizePermission("ManageDiscount")]

        public async Task<IActionResult> Delete(int id)
        {
            return await _discountService.DeleteAsync(id) ? NoContent() : NotFound(new { message = "Discount not found" });
        }
    }
}
