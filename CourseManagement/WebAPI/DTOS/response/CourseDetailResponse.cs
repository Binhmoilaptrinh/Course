namespace WebAPI.DTOS.response
{
    public class CourseDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PreviewVideo { get; set; }
        public string ThumbnailImage { get; set; }
        public double Duration { get; set; }
        public double Price { get; set; }
        public int ChaptersCount { get; set; }
        public int LessonsCount { get; set; }
        public List<ChapterDetailPage> Chapters { get; set; }
    }
}
