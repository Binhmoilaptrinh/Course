using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using WebAPI.Utilities;
using Net.payOS.Types;
using WebAPI.DTOS.request;

namespace WebAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ECourseContext _eCourseContext;
        private readonly PaymentHelper _paymentHelper;

        public PaymentService(ECourseContext eCourseContext, IConfiguration configuration)
        {
            _eCourseContext = eCourseContext;
            _paymentHelper = new PaymentHelper(configuration);
        }

        public async Task<string> CreatePaymentUrl(int courseId)
        {
            var course = await _eCourseContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Generate unique order code
            int price = (int)(course.Price * 100); // Convert to cents (if needed)

            var items = new List<ItemData>
            {
                new ItemData(course.Title, price, 1) // Assuming 1 quantity per course
            };

            return await _paymentHelper.GetLinkAsync(orderCode, price, items);
        }

        public async Task<Payment> UpdatePayment(PaymentRequest request)
        {
            var course = await _eCourseContext.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            Payment pay = new Payment()
            {
                Amount = (decimal)course.Price,
                PaymentDate = DateTime.Now,
                TransactionId = request.OrderCode.ToString(),
                IsSuccessful = request.IsSuccess,
                UserId = request.UserId,
                Status = request.IsSuccess ? 1 : 0,
                PaymentMethod = "PayOS",
                CourseId = request.CourseId
            };
            _eCourseContext.Payments.Add(pay);
            await _eCourseContext.SaveChangesAsync();
            return pay;
        }
    }
}
