using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ECourseContext _context;

        public QuestionRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }
    }
}

