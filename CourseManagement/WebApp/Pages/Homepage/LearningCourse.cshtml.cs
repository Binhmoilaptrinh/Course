using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Security.Claims;
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
        public LessonDetailResponseAdmin CurrentLesson { get; set; }
        public int CourseId { get; set; }
        public LessonProgress ProgressLesson { get; set; }
        public bool IsDoingQuizz { get; set; }

        public async Task<IActionResult> OnGetAsync(int courseId, int lessonId, int userId, string quizz = "")
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

            try
            {
                CourseId = courseId;

                // First, fetch the current lesson
                var lessonResponse = await _httpClient.GetAsync($"https://api.2handshop.id.vn/api/Lesson/{lessonId}");
                if (!lessonResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Failed to fetch current lesson.");
                }

                var lessonContent = await lessonResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(lessonContent))
                {
                    return BadRequest("Lesson content is empty.");
                }

                CurrentLesson = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(
                    lessonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Handle Video and Quizz types right after getting CurrentLesson
                if (CurrentLesson.Type.Equals("Video"))
                {
                    if (string.IsNullOrWhiteSpace(progressContent))
                    {
                        // Call EnrollLesson API if progress is empty
                        var enrollLessonRequest = new LessonEnroll { UserId = userId, LessonId = lessonId };
                        var enrollLessonResponse = await _httpClient.PostAsJsonAsync("https://api.2handshop.id.vn/api/CourseLearning/enroll", enrollLessonRequest);

                        if (!enrollLessonResponse.IsSuccessStatusCode)
                        {
                            return BadRequest("Failed to enroll in the lesson.");
                        }
                    }
                }
                if (CurrentLesson.Type.Equals("Quizz"))
                {
                    if (string.IsNullOrWhiteSpace(progressContent) && quizz.Equals("start")) //start
                    {
                        var enrollLessonRequest = new LessonEnroll { UserId = userId, LessonId = lessonId };
                        var enrollLessonResponse = await _httpClient.PostAsJsonAsync("https://api.2handshop.id.vn/api/CourseLearning/enroll", enrollLessonRequest);

                        if (!enrollLessonResponse.IsSuccessStatusCode)
                        {
                            return BadRequest("Failed to enroll in the lesson.");
                        }

                        IsDoingQuizz = true;
                    }

                    if (ProgressLesson != null && quizz.Equals("resume")) // resume
                    {
                        if (!validateTime(ProgressLesson.UpdatedAt ?? ProgressLesson.CreatedAt, ProgressLesson.CountDoing ?? 1))
                        {
                            TempData["messQuizz"] = "B?n không ?? ?i?u ki?n ?? làm bài quizz này";
                        }
                        else
                        {
                            ProgressLesson.CountDoing = ProgressLesson.CountDoing++;
                        }
                    }
                }


                LessonProgressResponse = JsonSerializer.Deserialize<LessonProgressResponse>(
                    progressContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );


                // Fetch chapters
                var apiUrl = $"https://api.2handshop.id.vn/api/Course/Chapters/{courseId}";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Chapters = JsonSerializer.Deserialize<List<ChapterDTO>>(jsonResponse,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format in response.");
            }

            return Page();
        }
        public bool validateTime(DateTime lastDate, int count)
        {
            //quy dinh m?i l?n làm quizz s? t?ng lên 5p
            TimeSpan requiredInterval = TimeSpan.FromMinutes(count * 1);

            DateTime currentDate = DateTime.Now;

            return (currentDate - lastDate) >= requiredInterval;
        }

    }
}