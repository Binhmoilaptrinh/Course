using static System.Runtime.InteropServices.JavaScript.JSType;
using WebAPI.Models;

namespace WebAPI.DTOS.response
{
    public class CourseClientDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Thumbnail { get; set; }
        public double Price { get; set; }
        public int Enrollments { get; set; }
        public int Lessons { get; set; }
        public float Durations { get; set; }
    }
}
