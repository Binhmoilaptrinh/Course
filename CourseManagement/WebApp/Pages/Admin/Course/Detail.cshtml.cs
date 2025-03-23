using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using WebAPI.DTOS.response;

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
        public CourseDetailAdmin CourseDetail { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // Tr? v? l?i n?u id không h?p l?
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/Course/Detail/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                CourseDetail = JsonSerializer.Deserialize<CourseDetailAdmin>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
            else
            {
                return NotFound(); 
            }
        }
    }
}
