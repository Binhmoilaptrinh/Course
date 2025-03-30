namespace WebApp.Models
{
    public class ProgressLessonUpdate
    {
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public float ProgressPercentage { get; set; }
        public int CourseId { get; set; }
    }
}
