namespace WebAPI.DTOS.response
{
    public class ChapterDetailPage
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int LessonsCount { get; set; }
        public List<LessonDetail>? LessonDetail { get; set; }
    }
}
