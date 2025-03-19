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
            decimal Price, int Cate, int Limit, string Desc)
        {
            Console.WriteLine(PreviewVideo);
            Console.WriteLine(Thumbnail);
            Console.WriteLine(Title);
            Console.WriteLine(Price);
            Console.WriteLine(Cate);
            Console.WriteLine(Limit);
            Console.WriteLine(Desc);

            var requestContent = new MultipartFormDataContent();

            if (PreviewVideo != null)
            {
                var stream = PreviewVideo.OpenReadStream();
                var videoContent = new StreamContent(stream);
                videoContent.Headers.ContentType = new MediaTypeHeaderValue(PreviewVideo.ContentType);
                requestContent.Add(videoContent, "PreviewVideo", PreviewVideo.FileName);
            }

            if (Thumbnail != null)
            {
                var stream = Thumbnail.OpenReadStream();
                var imageContent = new StreamContent(stream);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(Thumbnail.ContentType);
                requestContent.Add(imageContent, "Thumbnail", Thumbnail.FileName);
            }

            requestContent.Add(new StringContent(Title), "Title");
            requestContent.Add(new StringContent(Price.ToString()), "Price");
            requestContent.Add(new StringContent(Cate.ToString()), "Cate");
            requestContent.Add(new StringContent(Limit.ToString()), "Limit");
            requestContent.Add(new StringContent(Desc), "Desc");

            var response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Course", requestContent);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Admin/Course/CourseManageModel"); // Chuyển hướng nếu thành công
            }
            else
            {
                ModelState.AddModelError("", "Lỗi khi gửi dữ liệu đến API.");
                return RedirectToPage();
            }

            //using (var content = new MultipartFormDataContent())
            //{
            //    // Thêm video
            //    using (var stream = PreviewVideo.OpenReadStream())
            //    {
            //        var videoContent = new StreamContent(stream);
            //        videoContent.Headers.ContentType = new MediaTypeHeaderValue(PreviewVideo.ContentType);
            //        content.Add(videoContent, "PreviewVideo", PreviewVideo.FileName);
            //    }

            //    // Thêm ảnh thumbnail
            //    using (var stream = Thumbnail.OpenReadStream())
            //    {
            //        var imageContent = new StreamContent(stream);
            //        imageContent.Headers.ContentType = new MediaTypeHeaderValue(Thumbnail.ContentType);
            //        content.Add(imageContent, "Thumbnail", Thumbnail.FileName);
            //    }

            //    // Thêm các dữ liệu khác
            //    content.Add(new StringContent(Title), "Title");
            //    content.Add(new StringContent(Price.ToString()), "Price");
            //    content.Add(new StringContent(Cate.ToString()), "Cate");
            //    content.Add(new StringContent(Limit.ToString()), "Limit");
            //    content.Add(new StringContent(Desc), "Desc");

            //    // Gửi request
            //    var response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Course", content);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return RedirectToPage("/Admin/Course/CourseManageModel"); // Chuyển hướng nếu thành công
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Lỗi khi gửi dữ liệu đến API.");
            //        return RedirectToPage();
            //    }
            //}
        }
    }
}
