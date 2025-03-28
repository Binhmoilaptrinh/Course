using WebAPI.DTOS.request;

namespace WebAPI.Services.Interfaces
{
    public interface IFinanceService
    {
        Task<double> CalculateRevenueAsync(int month, int year);

        Task UpdateOrCreateFinanceRecordAsync(FinanceForUpdate finance);

        Task<double> GetRevenueAsync(int month, int year);
        Task<double> GetFeesAsync(int month, int year);
    }
}
