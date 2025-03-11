using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
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

        [EnableQuery]
        [HttpGet]
        public IQueryable<CourseAdminResponseDto> GetAllCourseManage()
        {
            return _courseService.GetAllCourse();
        }

        [HttpPost]
        public async Task<ActionResult<CourseAdminResponseDto>> CreateCourse([FromForm] CourseRequestDto course)
        {
            var courses = await _courseService.CreateCourseAsync(course);
            return Ok(courses);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseAdminResponseDto>> UpdateCourse(int id, [FromForm] CourseRequestDto course)
        {
            var courses = await _courseService.UpdateCourseAsync(id, course);
            return Ok(courses);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CourseAdminResponseDto>> GetCourse(int id)
        {
            var courses = await _courseService.GetCourseByIdAsync(id);
            return Ok(courses);
        }
    }
}
