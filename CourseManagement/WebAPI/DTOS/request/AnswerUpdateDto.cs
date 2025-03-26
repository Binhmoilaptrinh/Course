namespace WebAPI.DTOS.request
{
    public class AnswerUpdateDto
    {
        public int Id { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }
}
