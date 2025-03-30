using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CoursePayment> CreatePaymentUrl(int courseId, int userId);
        Task<Payment> UpdatePayment(long orderCode);

        Task<List<PaymentListResponse>> SearchPaymentsAsync(DateTime? fromDate, DateTime? toDate, string? orderNumber, int? status);
    }
}
