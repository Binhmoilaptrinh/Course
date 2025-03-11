namespace WebAPI.DTOS.request
{
    public class QuestionRequestDto
    {
        public int LessonId { get; set; }

        public string QuestionText { get; set; }

        public List<AnswerRequestDto> AnswersDto { get; set; }
    }
}
