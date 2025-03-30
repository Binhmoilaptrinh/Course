using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebAPI.DTOS.response;

namespace WebApp.Pages.Admin.CertificateTemplate
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public CertificateTemplateRes Certificate { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/template");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Certificate = JsonSerializer.Deserialize<CertificateTemplateRes>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile Certificate)
        {
            if (Certificate == null || Certificate.Length == 0)
            {
                ModelState.AddModelError("certificate", "Vui lòng chọn file PDF.");
                return Page();
            }

            using var requestContent = new MultipartFormDataContent();

            if (Certificate != null)
            {
                byte[] data;
                using (var br = new BinaryReader(Certificate.OpenReadStream()))
                {
                    data = br.ReadBytes((int)Certificate.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "cerPdf", Certificate.FileName);
            }

            var response = await _httpClient.PostAsync("http://localhost:5000/upload/template", requestContent);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý nếu cần lưu certificate vào view model
                return RedirectToPage("/Admin/CertificateTemplate/Create");
            }

            ModelState.AddModelError("", "Có lỗi xảy ra khi tải lên.");
            return Page();
        }

    }
}
