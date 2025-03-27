namespace WebApp.Models
{
    public class LessonResponseAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChapterId { get; set; }
        public string? VideoUrl { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string? Content { get; set; }
        public float? Duration { get; set; }
        public float? Passing { get; set; }
    }
}
