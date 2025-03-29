using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/chatgpt")] // Định nghĩa route của controller là "api/chatgpt"
    public class ChatGPTController : ControllerBase
    {
        private readonly ECourseContext _context; // Database context để truy vấn dữ liệu
        private readonly ChatClient _chatClient;  // Client để kết nối với OpenAI

        // Constructor để inject database context
        public ChatGPTController(ECourseContext context)
        {
            _context = context; // Lưu context để dùng trong API
            _chatClient = new ChatClient(model: "gpt-4o", apiKey: ""); // Khởi tạo ChatGPT client với API key
        }

        [HttpGet("ask")] // Định nghĩa API endpoint: GET /api/chatgpt/ask
        public async Task<IActionResult> AskChatGPT(string question)
        {
            // Lấy danh sách khóa học từ database và kèm thông tin danh mục
            var courses = await _context.Courses
                .Include(c => c.Category) // Lấy thông tin danh mục (Category)
                .Select(c => new
                {
                    c.Id,
                    c.Title,        // Tiêu đề khóa học
                    c.Price,        // Giá khóa học
                    c.Thumbnail,    // Ảnh khóa học
                    c.Description,  // Mô tả khóa học
                    c.CreatedAt,    // Ngày tạo khóa học
                    Category = c.Category.Name // Lấy tên danh mục của khóa học
                })
                .ToListAsync(); // Thực hiện truy vấn bất đồng bộ

            // Nếu không có khóa học nào thì trả về thông báo
            if (!courses.Any())
            {
                return NotFound("Hiện tại không có khóa học nào trong hệ thống.");
            }

            // Tạo dữ liệu khóa học dưới dạng text để gửi cho ChatGPT
            StringBuilder courseData = new StringBuilder();
            courseData.AppendLine("Dưới đây là danh sách các khóa học có trong hệ thống:");
            foreach (var course in courses)
            {
                courseData.AppendLine($"- {course.Title} (Giá: {course.Price} VND, Danh mục: {course.Category})");
            }

            // Tạo prompt cho ChatGPT
            string prompt = $"{courseData}\nNgười dùng hỏi: \"{question}\". Hãy trả lời câu hỏi này dựa trên danh sách khóa học trên.";

            // Gửi prompt đến ChatGPT và nhận câu trả lời
            ChatCompletion completion = _chatClient.CompleteChat(prompt);

            // Trả về kết quả phản hồi từ ChatGPT
            return Ok(completion.Content[0].Text);
        }
    }
}
