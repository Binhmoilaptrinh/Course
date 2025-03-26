using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebApp.Pages.Homepage
{
    public class LearningCourseModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LearningCourseModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public LessonProgressResponse LessonProgressResponse { get; set; }
        public List<ChapterDTO> Chapters { get; set; } = new List<ChapterDTO>();
        public int CourseId { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId, int lessonId, int userId)
        {
            var checkStatusDto = new EnrollmentRequestDto { UserId = userId, CourseId = courseId };

            // 1. Check Enrollment Status
            var statusResponse = await _httpClient.PostAsJsonAsync("https://api.2handshop.id.vn/api/Enrollment/CheckStatus", checkStatusDto);
            if (!statusResponse.IsSuccessStatusCode)
            {
                return BadRequest("Failed to check enrollment status.");
            }

            var statusContent = await statusResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(statusContent))
            {
                return BadRequest("Enrollment status response is empty.");
            }

            if (!int.TryParse(statusContent, out int status))
            {
                return BadRequest("Invalid response format for enrollment status.");
            }

            // 2. If status == 0, enroll the user
            if (status == 0)
            {
                var enrollResponse = await _httpClient.PostAsJsonAsync("https://api.2handshop.id.vn/api/Enrollment", checkStatusDto);
                if (!enrollResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to enroll in the course.");
                }
            }

            // 3. Fetch Lesson Progress
            var progressResponse = await _httpClient.GetAsync($"https://api.2handshop.id.vn/api/CourseLearning/lesson-progress?lessonId={lessonId}&userId={userId}");
            var progressContent = await progressResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(progressContent))
            {
                // Call EnrollLesson API if progress is empty
                var enrollLessonRequest = new LessonEnroll { UserId = userId, LessonId = lessonId };
                var enrollLessonResponse = await _httpClient.PostAsJsonAsync("https://api.2handshop.id.vn/api/CourseLearning/enroll", enrollLessonRequest);

                if (!enrollLessonResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to enroll in the lesson.");
                }

                // Retry fetching lesson progress
                progressResponse = await _httpClient.GetAsync($"https://api.2handshop.id.vn/api/CourseLearning/lesson-progress?lessonId={lessonId}&userId={userId}");
                progressContent = await progressResponse.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(progressContent))
                {
                    return BadRequest("Lesson progress response is still empty after enrollment.");
                }
            }

            try
            {
                CourseId = courseId;
                LessonProgressResponse = JsonSerializer.Deserialize<LessonProgressResponse>(
                    progressContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var apiUrl = $"https://api.2handshop.id.vn/api/Course/Chapters/{courseId}";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Chapters = JsonSerializer.Deserialize<List<ChapterDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format in lesson progress response.");
            }

            return Page();
        }

    }
}
