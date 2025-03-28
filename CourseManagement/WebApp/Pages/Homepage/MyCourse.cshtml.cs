using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WebApp.Models;

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
                return BadRequest(); // Tr? v? l?i n?u id không h?p l?
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/Enrollment/GetMyCourse?userId={id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            MyCourseResponse = JsonSerializer.Deserialize<List<MyCourseResponse>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            foreach (var enrollment in MyCourseResponse)
            {
                if (enrollment.Progress == 100)
                {
                    var certificateUrl = await GetCertificateUrlAsync(enrollment.EnrollmentId);

                    if (string.IsNullOrEmpty(certificateUrl))
                    {
                        CertificateUrls[enrollment.EnrollmentId] = await CreateCertificateAsync(enrollment);
                    }

                    else
                    {
                        CertificateUrls[enrollment.EnrollmentId] = certificateUrl;
                    }
                }
            }

            return Page();
        }

        private async Task<string?> GetCertificateUrlAsync(int enrollmentId)
        {
            var apiUrlCertificate = $"https://api.2handshop.id.vn/api/Certificate?enrollmentId={enrollmentId}";
            var response = await _httpClient.GetAsync(apiUrlCertificate);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        private async Task<string?> CreateCertificateAsync(MyCourseResponse enrollment)
        {
            var createCertificateUrl = "https://api.2handshop.id.vn/api/Certificate";
            var certificateRequest = new
            {
                enrollmentID = enrollment.EnrollmentId,
                userName = enrollment.UserName,
                courseName = enrollment.Title
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(certificateRequest), Encoding.UTF8, "application/json");
            var postResponse = await _httpClient.PostAsync(createCertificateUrl, jsonContent);

            if (postResponse.IsSuccessStatusCode)
            {
                var createdCertResponse = await postResponse.Content.ReadAsStringAsync();
                var createdCertificate = JsonSerializer.Deserialize<Certificate>(createdCertResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return createdCertificate?.CertificateUrl;
            }

            return null;
        }
    }
}
