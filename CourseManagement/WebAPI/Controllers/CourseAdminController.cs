using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
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

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IQueryable<CourseAdminResponseDto>>> Get()
        {
            var courses = await _courseService.GetAllCourse();
            return Ok(courses);
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
        public async Task<ActionResult<CourseDetailResponseDto>> GetCourseDetail(int id)
        {
            var courses = await _courseService.GetCourseByIdAsync(id);
            return Ok(courses);
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult<CourseDetailAdmin>> GetCourseDetailAdmin(int id)
        {
            var courses = await _courseService.GetCourseDetailAdmin(id);
            return Ok(courses);
        }
        [HttpGet("Chapters/{id}")]
        public async Task<ActionResult<List<ChapterDTO>>> GetChaptersAdmin(int id,int userId)
        {
            var chapters = await _courseService.GetChapters(id, userId);
            return Ok(chapters);
        }
    }
}
