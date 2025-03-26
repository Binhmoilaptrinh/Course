namespace WebAPI.DTOS.response
{
    public class ChapterDTO
    {
        public int? Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int LessonCount { get; set; }
        public double? Duration { get; set; }
        public List<LessonDTO>? Lessons { get; set; }
    }
}
