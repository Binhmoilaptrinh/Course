using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebApp.Pages.Homepage
{
    public class MyCourseModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public MyCourseModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<MyCourseResponse> MyCourseResponse { get; set; } = new();
        public Dictionary<int, string> CertificateUrls { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); 
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/Enrollment/GetMyCourse?userId={id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            MyCourseResponse = JsonSerializer.Deserialize<List<MyCourseResponse>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            return Page();
        }
    }
}
