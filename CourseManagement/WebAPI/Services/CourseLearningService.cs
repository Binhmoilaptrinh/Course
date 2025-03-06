using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CourseLearningService : ICourseLearningService
    {
        private readonly ECourseContext _eCourseContext;
        public CourseLearningService(ECourseContext eCourseContext)
        {
            _eCourseContext = eCourseContext;
        }
        public async Task<CourseLearningResponseDTO> GetCourseLearning(int courseId)
        {
            var course = await _eCourseContext.Courses.Select(x => new CourseLearningResponseDTO()
            {
                Id = x.Id,
                Name = x.Title,
                Chapters = _eCourseContext.Chapters.Select(c=> new ChapterLearningResponse
                {
                    ChapterId = c.Id,
                    Name = c.Name,
                    CourseId = c.CourseId,
                    LessonCounted = _eCourseContext.Lessons.Where(l => l.ChapterId == c.Id).Count(),
                    ChapterDurations = _eCourseContext.Lessons
                              .Where(l => l.ChapterId == c.Id && l.Duration.HasValue)
                              .Sum(l => l.Duration.Value),
                    Lessons = _eCourseContext.Lessons.Where(k=>k.ChapterId == c.Id).Select(n=> new LessonLearningResponse
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Type = n.Type,
                        VideoUrl = n.VideoUrl,
                        Status = n.Status,
                        Content = n.Content,
                        Duration = n.Duration,
                        Passing = n.Passing
                    }).ToList()
                }).ToList()
            }).FirstOrDefaultAsync(y=>y.Id == courseId);
            return course;
        }
    }
}
