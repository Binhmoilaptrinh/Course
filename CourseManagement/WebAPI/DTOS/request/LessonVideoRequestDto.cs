namespace WebAPI.DTOS.request
{
    public class LessonVideoRequestDto
    {
        public string Name { get; set; }

        public int ChapterId { get; set; }

        public string Type { get; set; }//video
        public IFormFile? Video { get; set; }
        public string Status { get; set; }

        public string? Content { get; set; }

        public float? Duration { get; set; }//time learn video

        public float? Passing { get; set; }//pass of video
    }
}
