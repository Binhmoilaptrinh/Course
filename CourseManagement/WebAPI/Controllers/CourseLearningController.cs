using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;
using System.Threading.Tasks;

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

        // Get course learning by courseId
        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseLearningResponseDTO>> GetCourseLearning(int courseId)
        {
            var courses = await _courseLearningService.GetCourseLearning(courseId);
            return Ok(courses);
        }

        // Get lesson progress by lessonId and userId
        [HttpGet("lesson-progress")]
        public async Task<ActionResult<LessonProgressResponse>> GetLessonProgress([FromQuery] int lessonId, [FromQuery] int userId)
        {
            var lessonProgress = await _courseLearningService.GetLessonProgress(lessonId, userId);
            return Ok(lessonProgress);
        }
        [HttpPost("latest-lesson")]
        public async Task<ActionResult<int>> GetLatestLEsson(LessonProgressLatest lessonProgress)
        {
            var latestLesson = await _courseLearningService.GetLatestLesson(lessonProgress);
            return Ok(latestLesson);
        }

        // Enroll in a lesson
        [HttpPost("enroll")]
        public async Task<ActionResult<LessonProgress>> EnrollLesson([FromBody] LessonEnroll enroll)
        {
            var lessonProgress = await _courseLearningService.EnrollLesson(enroll);
            return Ok(lessonProgress);
        }

        // Update lesson progress
        [HttpPost("update-progress")]
        public async Task<ActionResult<LessonProgress>> UpdateLessonProgress([FromBody] ProgressLessonUpdate progress)
        {
            var lessonProgress = await _courseLearningService.UpdateProgressLesson(progress);
            return Ok(lessonProgress);
        }
    }
}
