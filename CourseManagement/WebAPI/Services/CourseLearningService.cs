using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
                Passing = 0,
                HighestMark = 0
            };
            _eCourseContext.Add(lessonProgress);
            await _eCourseContext.SaveChangesAsync();
            return lessonProgress;

        }

        public async Task<LessonProgress> UpdateProgressLesson(ProgressLessonUpdate progress)
        {
            var progressLesson = await _eCourseContext.LessonProgresses.FirstOrDefaultAsync(x => x.LessonId == progress.LessonId && x.UserId == progress.UserId);
            progressLesson.ProgressPercentage = progress.ProgressPercentage;
            var passing = await _eCourseContext.Lessons.Where(x => x.Id == progress.LessonId).Select(x => x.Passing).FirstOrDefaultAsync();
            if (progressLesson.ProgressPercentage > passing)
            {
                progressLesson.Passing = 1;
            }
            if (progressLesson.Passing == 1)
            {
                var enrollment = await _eCourseContext.Enrollments.Where(x => x.UserId == progress.UserId && x.CourseId == progress.CourseId).FirstOrDefaultAsync();
                if (enrollment != null)
                {
                    int count = _eCourseContext.Lessons.Where(x => x.Chapter.Course.Id == progress.CourseId).Count();

                    // Calculate the total number of lessons in the course (from all chapters)
                    int countLesson = _eCourseContext.LessonProgresses
            .Where(lp => lp.UserId == progress.UserId
                 && lp.Passing == 1
                 && lp.Lesson.Chapter.CourseId == progress.CourseId)
            .Count();

                    // Calculate the number of passing lessons in the course


                    // Update the enrollment progress based on the ratio of passing lessons to total lessons
                    enrollment.Progress = (double)countLesson / count * 100;
                    _eCourseContext.Enrollments.Update(enrollment);
                    await _eCourseContext.SaveChangesAsync();
                }
            }
            progressLesson.CountDoing = progressLesson.CountDoing + 1;
            if (progressLesson.ProgressPercentage > progressLesson.HighestMark)
            {
                progressLesson.HighestMark = progressLesson.ProgressPercentage;
            }
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
                HighestMark = y.HighestMark,
                Passing = y.Passing,
                CountDoing = y.CountDoing,
                CreatedAt = y.CreatedAt,
                UpdatedAt = y.UpdatedAt
            }).FirstOrDefaultAsync(x => x.LessonId == lessonId && x.UserId == userId);
            return lessonProgress;
        }

        public async Task<int> GetLatestLesson(LessonProgressLatest lessonProgress)
        {
            // Get all lesson IDs in the specified course
            var lessonIdsInCourse = await _eCourseContext.Courses
                .Where(f => f.Id == lessonProgress.CourseId)
                .SelectMany(c => c.Chapters.SelectMany(ch => ch.Lessons.Select(l => l.Id)))
                .ToListAsync();

            if (!lessonIdsInCourse.Any())
            {
                return 0; // Or throw an exception if no lessons exist
            }

            // Find the latest lesson the user has progressed in this course
            var lessonLatest = await _eCourseContext.LessonProgresses
                .Where(lp => lp.UserId == lessonProgress.UserId && lessonIdsInCourse.Contains(lp.LessonId))
                .OrderByDescending(lp => lp.LessonId) // Assuming there's a LastAccessed field
                .Select(lp => lp.LessonId)
                .FirstOrDefaultAsync();

            // If no progress exists, return the first lesson in the course
            return lessonLatest > 0 ? lessonLatest : lessonIdsInCourse.First();
        }

    }
}
