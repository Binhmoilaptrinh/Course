using WebAPI.DTOS.request;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentUrl(int courseId);
        Task<Payment> UpdatePayment(PaymentRequest request);
    }
}
