using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.DTOS.response;
using iText.Kernel.Pdf;  // Đảm bảo import thư viện này
using iText.Kernel.Pdf.Canvas.Parser;
using Ghostscript.NET.Rasterizer;
using System.IO;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
namespace WebApp.Pages.Homepage
{
    public class CourseDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        private readonly Cloudinary _cloudinary;
        public CourseDetailModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var account = new Account("doslvje9p", "111261246971633", "lMdfdvz3SsDnpHJA_WDBtRQmFKU");
            _cloudinary = new Cloudinary(account);
        }

        public string courseDes { get; set; } = "";

        public CourseDetailResponse CourseDetail { get; set; }
        public int HasAccess { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // Trả về lỗi nếu id không hợp lệ
            }

            var apiUrl = $"http://localhost:5000/api/HomePage/GetCourseDetail?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                CourseDetail = JsonSerializer.Deserialize<CourseDetailResponse>(jsonResponse, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
                //CourseDetail.Description = await UploadPdfAndConvertToImage(CourseDetail.Description);
                return Page();
            }
            else
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy khóa học
            }
        }

        //public string ConvertPdfToText(string pdfPath)
        //{
        //    StringBuilder text = new StringBuilder();

        //    using (PdfReader reader = new PdfReader(pdfPath))
        //    using (PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))  // Chỉ định rõ namespace
        //    {
        //        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        //        {
        //            text.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
        //            text.Append("<br/>");
        //        }
        //    }
        //    return $"<html><body>{text}</body></html>";
        //}


        //public async Task<string> UploadPdfAndConvertToImage(string pdfUrl)
        //{
        //    var uploadParams = new ImageUploadParams()
        //    {
        //        File = new FileDescription(pdfUrl),
        //        Format = "jpg"
        //    };
        //    var uploadResult = _cloudinary.Upload(uploadParams);
          
        //    return uploadResult.SecureUrl.ToString(); // Trả về link ảnh
        //}
    }
}
