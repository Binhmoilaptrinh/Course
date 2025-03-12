using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.response;
using WebAPI.Mappings;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ECourseContext _context;

        public CourseRepository(ECourseContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task<IEnumerable<Course>> GetAllAsync()
        //{
        //    var courses = await _context.Courses.Include(x => x.Category).ToListAsync();
        //    return courses;
        //}
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(x => x.Category).ToListAsync();
        }


        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(x => x.Category)
                .Include(x => x.Chapters)
                .ThenInclude(x => x.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsExistByIdAsync(int id)
        {
           return await _context.Courses.AnyAsync(x => x.Id == id);
        }

        public async Task<Course> UpdateAsync(Course course)
        {
            _context.Entry(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return course;
        }
    }
}

