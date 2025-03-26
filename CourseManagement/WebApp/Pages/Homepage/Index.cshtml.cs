using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Homepage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<CourseList> ProCourses { get; set; } = new();
        public List<CourseList> FreeCourses { get; set; } = new();

        public async Task OnGetAsync()
        {
            var apiUrl = "https://api.2handshop.id.vn/api/HomePage/GetProCourses";
            var response = await _httpClient.GetAsync(apiUrl);

            var apiUrlFreeCourse = "https://api.2handshop.id.vn/api/HomePage/GetFreeCourses";
            var responseFreeCourse = await _httpClient.GetAsync(apiUrlFreeCourse);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                ProCourses = JsonSerializer.Deserialize<List<CourseList>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            if (responseFreeCourse.IsSuccessStatusCode)
            {
                var jsonResponse = await responseFreeCourse.Content.ReadAsStringAsync();
                FreeCourses = JsonSerializer.Deserialize<List<CourseList>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }

    public class CourseList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Category { get; set; }
        public int Enrollments { get; set; }
        public int Lessons { get; set; }
        public decimal Price { get; set; }
    }
    }
