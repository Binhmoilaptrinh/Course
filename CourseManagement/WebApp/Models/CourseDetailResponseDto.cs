namespace WebApp.Models
{
    public class CourseDetailResponseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }

        //co the null
        public TimeSpan? Duration { get; set; }
        public string Status { get; set; }
        public string PreviewVideo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int? LimitDay { get; set; }

        public List<ChapterDetailResponse>? ChapterResponse { get; set; }
    }
}
