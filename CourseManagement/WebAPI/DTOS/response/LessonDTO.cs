using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOS.response
{
    public class LessonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Duration { get; set; }
        public bool IsPassed { get; set; }
        public string Type { get; set; }
    }
}
