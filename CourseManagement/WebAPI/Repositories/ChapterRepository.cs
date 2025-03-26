using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ECourseContext _context;

        public ChapterRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<Chapter> CreateAsync(Chapter chapter)
        {
            _context.Chapters.Add(chapter); 
            await _context.SaveChangesAsync();
            return chapter;
        }

        public async Task DeleteAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            return await _context.Chapters.ToListAsync();
        }

        public async Task<Chapter> GetByIdAsync(int id)
        {
            return await _context.Chapters.FindAsync(id);
        }

        public async Task<Chapter> UpdateAsync(Chapter chapter)
        {
            _context.Entry(chapter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return chapter;
        }
    }
}
