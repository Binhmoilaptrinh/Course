namespace WebAPI.Models
{
    public class LessonProgress
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public float ProgressPercentage { get; set; }

        public float? HighestMark { get; set; }
        public float TimeSpent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? Status { get; set; }
        public int? Passing { get; set; }
        public int? CountDoing { get; set; }
    }
}
