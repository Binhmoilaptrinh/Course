using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebApp.Pages.Admin.Course
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var response = await _httpClient.GetAsync("https://api.2handshop.id.vn/api/Category");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Categories = JsonSerializer.Deserialize<List<Category>>(jsonString);
            }
            else
            {
                Categories = new List<Category>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile PreviewVideo, IFormFile Thumbnail, string Title, 
            decimal Price, int Cate, int Limit, string Desc, int CreateBy)
        {

            using var requestContent = new MultipartFormDataContent();

            if (PreviewVideo != null)
            {
                byte[] data;
                using (var br = new BinaryReader(PreviewVideo.OpenReadStream()))
                {
                    data = br.ReadBytes((int)PreviewVideo.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "PreviewVideo", PreviewVideo.FileName);
            }

            if (Thumbnail != null)
            {
                byte[] data;
                using (var br = new BinaryReader(Thumbnail.OpenReadStream()))
                {
                    data = br.ReadBytes((int)Thumbnail.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Thumbnail", Thumbnail.FileName);
            }

            requestContent.Add(new StringContent(Title ?? ""), "Title");
            requestContent.Add(new StringContent("pending"), "Status");
            requestContent.Add(new StringContent(Price.ToString()), "Price");
            requestContent.Add(new StringContent(CreateBy.ToString()), "UserId");
            requestContent.Add(new StringContent(Cate.ToString()), "CategoryId");
            requestContent.Add(new StringContent(Limit.ToString()), "LimitDay");
            requestContent.Add(new StringContent(Desc ?? ""), "Description");

            using var response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Course", requestContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/Course/List");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Lỗi khi gửi dữ liệu đến API: " + errorMessage);
                return Page();
            }
        }
    }
}
