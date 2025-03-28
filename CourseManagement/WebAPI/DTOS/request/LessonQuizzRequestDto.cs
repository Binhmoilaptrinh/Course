namespace WebAPI.DTOS.request
{
    public class LessonQuizzRequestDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public int ChapterId { get; set; }

        public string Status { get; set; }

        public string? Content { get; set; }

        public float? Duration { get; set; }//time doing quizz

        public float? Passing { get; set; }//pass of quizz

        public List<QuestionRequestDto>? QuestionsDto { get; set; }
    }
}
