using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.request;
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

        public async Task<LessonProgress> EnrollLesson(LessonEnroll enroll)
        {
            var lessonProgress = new LessonProgress()
            {
                LessonId = enroll.LessonId,
                UserId =  enroll.UserId,
                ProgressPercentage = 0,
                TimeSpent = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CountDoing = 0,
                Passing = 0
            };
            _eCourseContext.Add(lessonProgress);
            await _eCourseContext.SaveChangesAsync();
            return lessonProgress;

        }

        public async Task<LessonProgress> UpdateProgressLesson(ProgressLessonUpdate progress)
        {
            var progressLesson = await _eCourseContext.LessonProgresses.FirstOrDefaultAsync(x => x.LessonId == progress.LessonId && x.UserId == progress.UserId);
            progressLesson.ProgressPercentage = progress.ProgressPercentage;
            progressLesson.Passing = progress.Passing;
            _eCourseContext.Update(progressLesson);
            await _eCourseContext.SaveChangesAsync();
            return progressLesson;
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

        public async Task<LessonProgressResponse> GetLessonProgress(int lessonId, int userId)
        {
            var lessonProgress = await _eCourseContext.LessonProgresses.Select(y=> new LessonProgressResponse
            {
                Id = y.Id,
                LessonId = y.LessonId,
                UserId = y.UserId,
                Status = y.Status,
                ProgressPercentage = y.ProgressPercentage,
                Passing = y.Passing,
                CountDoing = y.CountDoing
            }).FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == userId);
            return lessonProgress;
        }
    }
}
