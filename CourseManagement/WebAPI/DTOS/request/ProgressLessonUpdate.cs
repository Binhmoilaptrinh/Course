namespace WebAPI.DTOS.request
{
    public class ProgressLessonUpdate
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public float ProgressPercentage { get; set; }
    }
}
