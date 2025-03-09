using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpGet("GetMyCourse")]
        public async Task<ActionResult<List<MyCourseResponse>>> GetMyCourse(int userId)
        {
            var lessonProgress = await _enrollmentService.GetEnrollmentsAsync(userId);
            return Ok(lessonProgress);
        }
        [HttpPost("CheckStatus")]
        public async Task<ActionResult<int>> CheckStatus(EnrollmentRequestDto requestDto)
        {
            var status = await _enrollmentService.CheckStatusEnrollment(requestDto);
            return Ok(status);
        }
    }
}
