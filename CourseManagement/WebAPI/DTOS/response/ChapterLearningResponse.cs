namespace WebAPI.DTOS.response
{
    public class ChapterLearningResponse
    {
        public int ChapterId { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int LessonCounted { get; set; }
        public float ChapterDurations { get; set; }
        public List<LessonLearningResponse> Lessons { get; set; }
    }
}
