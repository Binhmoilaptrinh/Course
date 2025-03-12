namespace WebAPI.DTOS.response
{
    public class ChapterDetailResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }

        public List<LessonResponseAdmin>? LessonResponse { get; set; }
    }
}
