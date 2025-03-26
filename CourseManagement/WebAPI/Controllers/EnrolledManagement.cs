using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolledManagement : Controller
    {
        private readonly IEnrollmentAdminService _enrollmentAdminService;

        public EnrolledManagement(IEnrollmentAdminService enrollmentAdminService)
        {
            _enrollmentAdminService = enrollmentAdminService;
        }

        [HttpGet("GetEnrollmentList")]
        public async Task<IActionResult> GetEnrollmentList()
        {
            var enrollments = await _enrollmentAdminService.GetEnrollmentList();
            return Ok(enrollments);
        }

        [HttpGet("GetEnrollmentListByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<EnrollmentResponseDTO>>> GetEnrollmentListByCourseId(int courseId)
        {
            try
            {
                var enrollments = await _enrollmentAdminService.GetEnrollmentListByCourseId(courseId);
                if(enrollments == null || enrollments.Count() == 0)
                    return NotFound("No enrollments found for this course.");
                return Ok(enrollments);
            } catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error: " + e.Message);
            }
        }

        [HttpGet("EnrollmentListByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> EnrollmentListByCourseId(int userId)
        {
            try
            {
                var enrollments = await _enrollmentAdminService.GetListCustomerEnrollmentByUserId(userId);

                if(enrollments == null || enrollments.Count() == 0)
                {
                    return NotFound("No enrollments found for this user.");
                }

                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    }
}
