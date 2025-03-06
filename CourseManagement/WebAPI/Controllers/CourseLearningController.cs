using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.response;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLearningController : ControllerBase
    {
        private readonly ICourseLearningService _courseLearningService;

        public CourseLearningController(ICourseLearningService courseLearningService)
        {
            _courseLearningService = courseLearningService;
        }
        [HttpGet]
        public async Task<ActionResult<CourseLearningResponseDTO>> GetCourseLearning(int courseId)
        {
            var courses = await _courseLearningService.GetCourseLearning(courseId);
            return Ok(courses);
        }
    }
}
