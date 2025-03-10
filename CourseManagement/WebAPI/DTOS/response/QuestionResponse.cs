using WebAPI.Models;

namespace WebAPI.DTOS.response
{
    public class QuestionResponse
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string QuestionText { get; set; }

        public List<AnswerResponse> AnswerResponse { get; set; }
    }
}
