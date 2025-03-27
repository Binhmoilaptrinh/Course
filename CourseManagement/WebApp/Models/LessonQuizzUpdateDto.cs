namespace WebApp.Models
{
    public class LessonQuizzUpdateDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public int ChapterId { get; set; }

        public string Type { get; set; }//quizz
        public string Status { get; set; }

        public string? Content { get; set; }

        public float? Duration { get; set; }//time doing quizz

        public float? Passing { get; set; }//pass of quizz

        public List<QuestionUpdateDto>? QuestionsDto { get; set; }
    }
}
