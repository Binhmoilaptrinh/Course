using WebAPI.Models;

namespace WebAPI.DTOS.response
{
    public class ChapterResponse
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
