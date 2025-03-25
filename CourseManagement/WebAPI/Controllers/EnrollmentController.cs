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
        [HttpPost]
        public async Task<IActionResult> EnrollCourseFree([FromBody] EnrollmentRequestDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var isEnrolled = await _enrollmentService.EnrollCourse(requestDto);

                if (!isEnrolled)
                {
                    return BadRequest("Enrollment failed. The course might not exist or has restrictions.");
                }

                return Ok(new { Success = true, Message = "Enrollment successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while enrolling in the course: {ex.Message}");
            }
        }

        [HttpPost("CheckStatus")]
        public async Task<ActionResult<int>> CheckStatus(EnrollmentRequestDto requestDto)
        {
            var status = await _enrollmentService.CheckStatusEnrollment(requestDto);
            return Ok(status);
        }
    }
}
