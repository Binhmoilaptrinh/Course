using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IFileService _fileService;

        public CourseController(ICourseService courseService, IFileService fileService)
        {
            _courseService = courseService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseAdminResponseDto>>> GetAllCourseManage()
        {
            var courses = await _courseService.GetAllCourseAsync();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<ActionResult<CourseAdminResponseDto>> CreateCourse([FromForm] CourseCreateDto course)
        {
            var courses = await _courseService.CreateCourseAsync(course);
            return Ok(courses);
        }

    }
}
