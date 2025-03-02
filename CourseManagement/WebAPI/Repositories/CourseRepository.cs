using AutoMapper;
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

        public async Task<Course> createAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> CreateAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            return courses;

        }
    }
}
