using Microsoft.EntityFrameworkCore;
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

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task<Answer> UpdateAnswerAsync(Answer answer)
        {
            _context.Entry(answer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return answer;
        }
    }
}
