using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public List<MyCourseResponse> MyCourseResponse { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // Tr? v? l?i n?u id không h?p l?
            }

            var apiUrl = $"https://api.2handshop.id.vn/api/Enrollment/GetMyCourse?userId={id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                MyCourseResponse = JsonSerializer.Deserialize<List<MyCourseResponse>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var enrollment in MyCourseResponse)
                {
                    if (enrollment.Progress == 100)
                    {
                        var apiUrlCertificate = $"https://api.2handshop.id.vn/api/Enrollment/GetMyCourse?userId={id}";
                        var responseCertificate = await _httpClient.GetAsync(apiUrl);

                        if (certificate == null)
                        {
                            // Generate and upload if the certificate doesn't exist
                            Certificate certi = await _fileService.GenerateAndUploadCertificateAsync(enrollment.Id, enrollment.User.UserName, enrollment.Course.Title);
                            CertificateUrls[enrollment.Id] = certi.CertificateUrl;
                        }
                        else
                        {
                            CertificateUrls[enrollment.Id] = certificate;
                        }
                    }
                }
                return Page();
            }
            else
            {
                return NotFound(); 
            }

        }
    }
}
