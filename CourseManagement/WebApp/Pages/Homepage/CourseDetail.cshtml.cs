using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;


namespace WebApp.Pages.Homepage
{
    public class CourseDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public CourseDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public CourseDetailResponse CourseDetail { get; set; }
        public int HasAccess { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // Trả về lỗi nếu id không hợp lệ
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/HomePage/GetCourseDetail?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                CourseDetail = JsonSerializer.Deserialize<CourseDetailResponse>(jsonResponse, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

                return Page();
            }
            else
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy khóa học
            }
        }

    }
}
