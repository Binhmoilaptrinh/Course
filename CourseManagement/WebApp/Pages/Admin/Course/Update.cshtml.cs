using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Pages.Admin.Course
{
    public class UpdateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public UpdateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [BindProperty]
        public CourseDetailAdmin CourseDetail { get; set; }
        [BindProperty]
        public List<Category> Categories { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if(id == null)
            {
                return Page();
            }
            var response = await _httpClient.GetAsync("https://api.2handshop.id.vn/api/Course/Detail/" + id);
            var cateResponse = await _httpClient.GetAsync("https://api.2handshop.id.vn/api/Category");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonString = await response.Content.ReadAsStringAsync();
                CourseDetail = JsonSerializer.Deserialize<CourseDetailAdmin>(jsonString, options);

                var cateJsonString = await cateResponse.Content.ReadAsStringAsync();
                Categories = JsonSerializer.Deserialize<List<Category>>(cateJsonString, options);
            Console.WriteLine(CourseDetail);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile PreviewVideo, IFormFile Thumbnail, string Title,
            decimal Price, int Cate, int Limit, string Desc, int CourseId, int UpdateBy)
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
            requestContent.Add(new StringContent(UpdateBy.ToString()), "UserId");
            requestContent.Add(new StringContent(Cate.ToString()), "CategoryId");
            requestContent.Add(new StringContent(Limit.ToString()), "LimitDay");
            requestContent.Add(new StringContent(Desc ?? ""), "Description");
            Console.WriteLine(requestContent);
            using var response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Course/" + CourseId, requestContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                return Redirect("?id="+ CourseId);
            }
            else
            {
                TempData["ErrorMessage"] = "Cập nhật khóa học thất bại!";
                return Redirect("?id=" + CourseId);
            }
        }
    }
}
