namespace WebAPI.DTOS.request
{
    public class AnswerRequestDto
    {
        public int? QuestionId { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }
}
