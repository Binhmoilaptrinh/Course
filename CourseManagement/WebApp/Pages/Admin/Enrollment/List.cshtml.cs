using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
namespace WebApp.Pages.Admin.Enrollment
{
    public class ListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public IEnumerable<EnrollmentResponseDTO> enrollmentDtos { get; set; }
        public ListModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/EnrolledManagement/GetEnrollmentListByCourseId/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                enrollmentDtos = JsonSerializer.Deserialize<IEnumerable<EnrollmentResponseDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}
