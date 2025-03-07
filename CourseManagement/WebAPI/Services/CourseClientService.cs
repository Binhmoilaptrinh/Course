using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CourseClientService : ICourseClientService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly ECourseContext _context;
        public CourseClientService (ICourseRepository courseRepository, IMapper mapper, ECourseContext context)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _context = context;
        }
        //public async Task<IEnumerable<CourseClientDTO>> GetCourseListHomePageFree123()
        //{
        //    var courses = await _context.Courses.ToListAsync();
        //    return _mapper.Map<IEnumerable<CourseClientDTO>>(courses);
        //}

        public async Task<IEnumerable<CourseClientDTO>> GetCourseListHomePageFree()
        {
            var courseList = await _context.Courses.Select(x => new CourseClientDTO {
                Id = x.Id,
                Title = x.Title,
                Thumbnail = x.Thumbnail,
                Price = x.Price,
                Category = x.Category.Name,
                Enrollments = _context.Enrollments.Where(y => y.CourseId == x.Id).Count(),
                Lessons = _context.Lessons.Where(z => z.Chapter.CourseId == x.Id).Count(),
                Durations = _context.Lessons.Where(e => e.Chapter.CourseId == x.Id && e.Duration.HasValue).Sum(e => e.Duration.Value)
            }).Where(x => x.Price == 0).Take(8).ToListAsync();  
            return courseList;
        }

        public async Task<IEnumerable<CourseClientDTO>> GetProCourses()
        {
            var courseList = await _context.Courses.Select(x => new CourseClientDTO
            {
                Id = x.Id,
                Title = x.Title,
                Thumbnail = x.Thumbnail,
                Price = x.Price,
                Category = x.Category.Name,
                Enrollments = _context.Enrollments.Where(y => y.CourseId == x.Id).Count(),
                Lessons = _context.Lessons.Where(z => z.Chapter.CourseId == x.Id).Count(),
                Durations = _context.Lessons.Where(e => e.Chapter.CourseId == x.Id && e.Duration.HasValue).Sum(e => e.Duration.Value)
            }).Where(x => x.Price != 0).Take(8).ToListAsync();
            return courseList;
        }
    }
}
