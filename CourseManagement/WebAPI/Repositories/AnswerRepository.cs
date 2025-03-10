using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ECourseContext _context;

        public AnswerRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<Answer> AddAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }
    }
}
