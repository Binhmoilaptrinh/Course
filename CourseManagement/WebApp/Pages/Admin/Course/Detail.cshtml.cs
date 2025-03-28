
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Pages.Admin.Course
{
    public class DetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public List<ChapterDTO> Chapters { get; set; } = new List<ChapterDTO>();
        public CourseDetailResponseDto CourseDetail { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // Tr? v? l?i n?u id không h?p l?
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/Course/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                CourseDetail = JsonSerializer.Deserialize<CourseDetailResponseDto>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
            else
            {
                return NotFound(); 
            }
        }

        public async Task<IActionResult> OnPostAsync(int courseId, List<string> oldChapterName, List<int> oldChapterId, List<string> newChaptersName)
        {
            //update chapter
            for (int i = 0; i< oldChapterId.Count(); i++)
            {
                ChapterRequestDto chapter = new ChapterRequestDto
                {
                    Name = oldChapterName[i],
                    Status = "Active",
                    CourseId = courseId
                };
                using var jsonContent = new StringContent(JsonSerializer.Serialize(chapter), Encoding.UTF8, "application/json");

                using var response = await _httpClient.PutAsync("https://api.2handshop.id.vn/api/Chapter/" + oldChapterId[i], jsonContent);
            }

            //add chapter
            //update chapter
            foreach (var name in newChaptersName)
            {
                ChapterRequestDto chapter = new ChapterRequestDto
                {
                    Name = name,
                    Status = "Active",
                    CourseId = courseId
                };
                using var jsonContent = new StringContent(JsonSerializer.Serialize(chapter),  Encoding.UTF8, "application/json");

                using var response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Chapter", jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Lỗi khi save chapter!";
                    return Redirect("/Admin/Course/Detail/" + courseId);
                }
            }

            TempData["SuccessMessage"] = "Save chapter thành công!";
            return Redirect("/Admin/Course/Detail/" + courseId);
        }

        public async Task<IActionResult> OnGetDeleteChapter(int chapterId, int courseId)
        {
            using var response = await _httpClient.DeleteAsync("https://api.2handshop.id.vn/api/Chapter/" + chapterId);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đã xóa thành công chapter";
                return Redirect("/Admin/Course/Detail/" + courseId);
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa chapter thất bại.";
                return Redirect("/Admin/Course/Detail/" + courseId);
            }
        }
    }
}
