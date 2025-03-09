using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
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
        public async Task<ActionResult<LessonProgress>> CreatePayment(int courseId)
        {
            var lessonProgress = await _paymentService.CreatePaymentUrl(courseId);
            return Ok(lessonProgress);
        }
        [HttpPost("UpdateSuccess")]
        public async Task<ActionResult<Payment>> UpdateSuccess([FromBody] PaymentRequest request)
        {
            request.IsSuccess = true;
            var payment = await _paymentService.UpdatePayment(request);
            var enroll = new EnrollmentRequestDto()
            {
                UserId = request.UserId,
                CourseId = request.CourseId,
            };
            await _enrollmentService.EnrollCourse(enroll);
            return Ok(payment);
        }
        [HttpPost("UpdateFail")]
        public async Task<ActionResult<Payment>> UpdateFail([FromBody] PaymentRequest request)
        {
            request.IsSuccess = false;
            var payment = await _paymentService.UpdatePayment(request);
            return Ok(payment);
        }
    }
}
