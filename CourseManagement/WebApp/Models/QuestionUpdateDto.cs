namespace WebApp.Models
{
    public class QuestionUpdateDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerUpdateDto> AnswersDto { get; set; }
    }
}
