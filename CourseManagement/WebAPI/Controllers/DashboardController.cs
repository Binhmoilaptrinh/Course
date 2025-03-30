using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public DashboardController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] int month, [FromQuery] int year)
        {
            var totalRevenue = await _financeService.GetRevenueAsync(month, year);
            var totalFee = await _financeService.GetFeesAsync(month, year);

            var statistics = new
            {
                TotalCourses = 120, // Placeholder, replace with actual data
                ActiveUsers = 4500, // Placeholder, replace with actual data
                TotalRevenue = totalRevenue,
                TotalFee = totalFee,
                NewCoursesThisMonth = 10 // Placeholder, replace with actual data
            };

            return Ok(statistics);
        }

        [HttpGet("finance-chart")]
        public async Task<IActionResult> GetFinanceChart([FromQuery] int year)
        {
            var labels = new List<string>();
            var data = new List<double>();

            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                labels.Add(new DateTime(year, i, 1).ToString("MMM"));
                var revenue = await _financeService.GetRevenueAsync(i, year);
                var fees = await _financeService.GetFeesAsync(i, year);
                data.Add(revenue - fees);
            }

            return Ok(new { Labels = labels, Data = data });
        }

        [HttpPost("update-revenue")]
        public async Task<IActionResult> UpdateRevenue([FromBody] FinanceForUpdate finance)
        {
            var revenue = await _financeService.CalculateRevenueAsync(finance.Month, finance.Year);
            finance.Revenue = revenue;
            await _financeService.UpdateOrCreateFinanceRecordAsync(finance);

            return Ok(new { Message = "Revenue updated successfully" });
        }
    }
}
