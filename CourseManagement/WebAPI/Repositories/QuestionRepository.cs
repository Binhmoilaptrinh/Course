using Microsoft.EntityFrameworkCore;
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

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return question;
        }
    }
}

