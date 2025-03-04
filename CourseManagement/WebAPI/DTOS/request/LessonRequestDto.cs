namespace WebAPI.DTOS.request
{
    public class LessonRequestDto
    {
        public string Name { get; set; }

        public int ChapterId { get; set; }

        public string Type { get; set; }//video || quizz

        public string? VideoUrl { get; set; }//lesson video || null if the quizz
        public string Status { get; set; }

        public string? Content { get; set; }

        public float? Duration { get; set; }//time learn video || time doing quizz

        public float? Passing { get; set; }//pass of video || pass of quizz
    }
}
