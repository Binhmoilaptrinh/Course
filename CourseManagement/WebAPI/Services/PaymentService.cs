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
using WebAPI.DTOS.response;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<CoursePayment> CreatePaymentUrl(int courseId, int userId)
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
            var coursePay = new CoursePayment()
            {
                Title = course.Title,
                Thumbnail = course.Thumbnail,
                Description = course.Description,   
                Duration = _eCourseContext.Lessons
                        .Where(l => l.Chapter.CourseId == courseId && l.Duration.HasValue)
                        .Sum(l => l.Duration.Value),
                LessonCount = _eCourseContext.Lessons.Where(l => l.Chapter.CourseId == courseId).Count(),
                Price = price,
                Url = await _paymentHelper.GetLinkAsync(orderCode, price, items)
            };
            Payment pay = new Payment()
            {
                Amount = (decimal)course.Price,
                PaymentDate = DateTime.Now,
                TransactionId = orderCode.ToString(),
                IsSuccessful = false,
                UserId = userId,
                Status = 0,
                PaymentMethod = "PayOS",
                CourseId = courseId
            };
            _eCourseContext.Payments.Add(pay);
            await _eCourseContext.SaveChangesAsync();
            return coursePay;
            
        }



        public async Task<Payment> UpdatePayment(long orderCode)
        {
            var payment = await _eCourseContext.Payments.FirstOrDefaultAsync(x => x.TransactionId == orderCode.ToString());
            payment.IsSuccessful = true;
            payment.Status = 1;
            payment.PaymentDate = DateTime.Now;
            _eCourseContext.Payments.Update(payment);
            
            var enroll = new EnrollmentRequestDto
            {
                UserId = payment.UserId,
                CourseId = payment.CourseId,
            };

            // Kiểm tra trạng thái Enrollment
            int enrollmentStatus = await _enrollmentService.CheckStatusEnrollment(enroll);

            if (enrollmentStatus == 0)
            {
                await _enrollmentService.EnrollCourse(enroll);
            }
            else if (enrollmentStatus == 2)
            {
                await _enrollmentService.UpdateEnrollmentStatus(enroll); 
            }
            await _eCourseContext.SaveChangesAsync();
            return payment;
        }


        public async Task<List<PaymentListResponse>> SearchPaymentsAsync(DateTime? fromDate, DateTime? toDate, string? orderNumber, int? status)
        {
            var query = _eCourseContext.Payments.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate <= toDate.Value);
            }

            if (!string.IsNullOrEmpty(orderNumber))
            {
                query = query.Where(p => p.TransactionId.Contains(orderNumber));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            return await query.Select(p => new PaymentListResponse
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                TransactionId = p.TransactionId,
                IsSuccessful = p.IsSuccessful,
                CourseId = p.CourseId,
                UserId = p.UserId,
                PaymentMethod = p.PaymentMethod,
                Status = p.Status
            }).ToListAsync();
        }
    }
}
