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
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ECourseContext _eCourseContext;
        private readonly PaymentHelper _paymentHelper;
        private readonly IEnrollmentService _enrollmentService;

        public PaymentService(ECourseContext eCourseContext, IConfiguration configuration, IEnrollmentService enrollmentService)
        {
            _eCourseContext = eCourseContext;
            _paymentHelper = new PaymentHelper(configuration);
            _enrollmentService = enrollmentService;
        }

        public async Task<string> CreatePaymentUrl(int courseId, int userId)
        {
            var course = await _eCourseContext.Courses
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            // Kiểm tra trạng thái Enrollment bằng CheckStatusEnrollment
            int enrollmentStatus = await _enrollmentService.CheckStatusEnrollment(new EnrollmentRequestDto
            {
                CourseId = courseId,
                UserId = userId
            });

            long orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Generate unique order code

            // Nếu enrollment có status = 2, giảm giá 50%
            int price = (int)(course.Price * (enrollmentStatus == 2 ? 0.5 : 1));

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
