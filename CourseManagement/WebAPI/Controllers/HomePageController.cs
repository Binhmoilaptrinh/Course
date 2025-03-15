using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : Controller
    {

        private readonly ICourseClientService _courseClientService;
        public HomePageController (ICourseClientService courseClientService)
        {
            _courseClientService = courseClientService;
        }

        [HttpGet("GetFreeCourses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseClientDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Lấy danh sách khóa học miễn phí",
        Description = "Trả về danh sách tối đa 8 khóa học có giá bằng 0"
        )]
        public async Task<ActionResult<IEnumerable<CourseClientDTO>>> GetCourseListHomePageFree()
        {
            var courses = await _courseClientService.GetCourseListHomePageFree();

            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found!"); // Trả về HTTP 404 nếu không có dữ liệu
            }

            return Ok(courses);
        }

        [HttpGet("GetProCourses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseClientDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Lấy danh sách khóa học pro",
        Description = "Trả về danh sách tối đa 8 khóa học pro"
        )]
        public async Task<ActionResult<IEnumerable<CourseClientDTO>>> GetProCourses()
        {
            var courses = await _courseClientService.GetProCourses();

            if (courses == null || !courses.Any())
            {
                return NotFound("No courses found!"); // Trả về HTTP 404 nếu không có dữ liệu
            }

            return Ok(courses);
        }
        [HttpGet("GetCourseDetail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CourseClientDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
        Summary = "Lấy danh sách khóa học pro",
        Description = "Trả về danh sách tối đa 8 khóa học pro"
        )]
        public async Task<ActionResult<CourseDetailResponse>> GetCourseDetail(int id)
        {
            var courses = await _courseClientService.GetCourseDetailHomepage(id);

            if (courses == null)
            {
                return NotFound("No courses found!"); // Trả về HTTP 404 nếu không có dữ liệu
            }

            return Ok(courses);
        }

    }
}
