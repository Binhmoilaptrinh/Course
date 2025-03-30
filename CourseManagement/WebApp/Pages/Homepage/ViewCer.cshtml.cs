using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Text.Json;
using WebAPI.DTOS.response;

namespace WebApp.Pages.Homepage
{
    public class ViewCerModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CertificateUserRes Certificate { get; set; }
        public ViewCerModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync(int enrollId = 0)
        {
            if(enrollId != 0)
            {
                Certificate = await GetCertificateUrlAsync(enrollId);
                if(Certificate != null)
                {
                    return Page();
                }
            }
            return RedirectToPage("./Index");
        }

        private async Task<CertificateUserRes> GetCertificateUrlAsync(int enrollmentId)
        {
            var apiUrlCertificate = $"http://localhost:5000/api/Certificate?enrollmentId={enrollmentId}";
            var response = await _httpClient.GetAsync(apiUrlCertificate);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if(!jsonResponse.IsNullOrEmpty())
                {
                    var cer = JsonSerializer.Deserialize<CertificateUserRes>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return cer;
                }
            }
            return null;
        }
    }
}
