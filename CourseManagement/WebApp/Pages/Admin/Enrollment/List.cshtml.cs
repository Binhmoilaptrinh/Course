using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.DTOS.response;

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

            var apiUrl = $"http://localhost:5298/api/EnrolledManagement/GetEnrollmentListByCourseId/{id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                enrollmentDtos = JsonSerializer.Deserialize<IEnumerable<EnrollmentResponseDTO>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
