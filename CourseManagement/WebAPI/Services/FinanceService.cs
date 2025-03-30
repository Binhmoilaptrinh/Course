using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAPI.DTOS.request;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly ECourseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FinanceService(ECourseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<double> CalculateRevenueAsync(int month, int year)
        {
            return (double)await _context.Payments
                .Where(p => p.PaymentDate.Month == month && p.PaymentDate.Year == year && p.IsSuccessful)
                .SumAsync(p => p.Amount);
        }

        public async Task UpdateOrCreateFinanceRecordAsync(FinanceForUpdate finance)
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userLogin = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (userLogin == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var financeRecord = await _context.Finances
                .FirstOrDefaultAsync(f => f.Month == finance.Month && f.Year == finance.Year);

            if (financeRecord != null)
            {
                financeRecord.Revenue = finance.Revenue;
                financeRecord.UpdatedAt = DateTime.Now;
                financeRecord.UpdatedBy = userLogin.UserId.ToString();
            }
            else
            {
                var newFinanceRecord = new Finance
                {
                    Month = finance.Month,
                    Year = finance.Year,
                    Revenue = finance.Revenue,
                    Type = "Course",
                    CreatedAt = DateTime.Now,
                    CreatedBy = userLogin.ToString(),
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = userLogin.ToString()
                };

                await _context.Finances.AddAsync(newFinanceRecord);
            }

            await _context.SaveChangesAsync();
        }


        public async Task<double> GetRevenueAsync(int month, int year)
        {
            return (double)await _context.Finances
                .Where(p => p.Month == month && p.Year == year)
                .SumAsync(p => p.Revenue);
        }

        // Calculates finance fees based on a fixed percentage or a flat fee per successful payment
        public async Task<double> GetFeesAsync(int month, int year)
        {
            var sum = await _context.Finances
                .Where(f => f.Month == month && f.Year == year)
                .SumAsync(f => (double?)f.Fee); // SumAsync returns nullable

            return sum ?? 0; // Return 0 if sum is null
        }

    }
}
