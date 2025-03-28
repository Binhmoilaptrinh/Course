using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IEnrollmentService _enrollmentService;
        public PaymentsController(IPaymentService paymentService, IEnrollmentService enrollmentService) 
        {
            _paymentService = paymentService;
            _enrollmentService = enrollmentService;
        }
        [HttpPost("CreatePayment")]
        public async Task<ActionResult<LessonProgress>> CreatePayment(ConfirmRequest request)
        {
            var lessonProgress = await _paymentService.CreatePaymentUrl(request.CourseId, request.UserId);
            return Ok(lessonProgress);
        }

        /*public async Task<ActionResult<Payment>> UpdateSuccess([FromBody] PaymentRequest request)
        {
            request.IsSuccess = true;
            var payment = await _paymentService.UpdatePayment(request);

            var enroll = new EnrollmentRequestDto
            {
                UserId = request.UserId,
                CourseId = request.CourseId,
            };

            // Kiểm tra trạng thái Enrollment
            int enrollmentStatus = await _enrollmentService.CheckStatusEnrollment(enroll);

            if (enrollmentStatus == 0)
            {
                // Nếu chưa đăng ký, thì đăng ký mới
                await _enrollmentService.EnrollCourse(enroll);
            }
            else if (enrollmentStatus == 2)
            {
                // Nếu trạng thái là 2, cập nhật Enrollment
                await _enrollmentService.UpdateEnrollmentStatus(enroll); // Cập nhật trạng thái thành 1 hoặc giá trị phù hợp
            }

            return Ok(payment);
        }*/

        [HttpPost("UpdateSuccess")]
        public async Task<ActionResult<Payment>> UpdateSuccess(long orderCode)
        {
            var payment = await _paymentService.UpdatePayment(orderCode);
            return Ok(payment);
        }

        [HttpGet("SearchPayments")]
        public async Task<ActionResult<PaymentListResponse>> UpdateSuccess(DateTime? fromDate, DateTime? toDate, string? orderNumber, int? status)
        {
            var payment = await _paymentService.SearchPaymentsAsync(fromDate, toDate, orderNumber, status);
            return Ok(payment);
        }

    }
}
