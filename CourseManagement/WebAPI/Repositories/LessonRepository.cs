using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ECourseContext _context;

        public LessonRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _context.Lessons
                .Include(x => x.Chapter)
                .Include(x => x.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();
        }

        public async Task<Lesson> GetByIdAsync(int id)
        {
            return await _context.Lessons.Include(x => x.Chapter)
                .Include(x => x.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Lesson> UpdateAsync(Lesson lesson)
        {
            _context.Entry(lesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return lesson;
        }
    }
}
