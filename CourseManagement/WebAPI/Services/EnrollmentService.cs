using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ECourseContext _eCourseContext;
        public EnrollmentService(ECourseContext eCourseContext)
        {
            _eCourseContext = eCourseContext;
        }

        public async Task<int> CheckStatusEnrollment(EnrollmentRequestDto enrollmentRequest)
        {
            var enrollmentStatus = await _eCourseContext.Enrollments
                .Where(x => x.CourseId == enrollmentRequest.CourseId && x.UserId == enrollmentRequest.UserId)
                .Select(x => (int?)x.Status) 
                .FirstOrDefaultAsync();

            return enrollmentStatus ?? 0; 
        }


        public async Task<bool> EnrollCourse(EnrollmentRequestDto enrollmentRequest)
        {
            var limitedDate = await _eCourseContext.Courses.Where(x => x.Id == enrollmentRequest.CourseId).Select(x=>x.LimitDay).FirstOrDefaultAsync();
            var enrollment = new Enrollment()
            {
                UserId = enrollmentRequest.UserId,
                CourseId = enrollmentRequest.CourseId,
                Progress = 0,
                Status = 1,
                EnrollmentDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddDays((double)limitedDate)
            };
            _eCourseContext.Enrollments.Add(enrollment);
            await _eCourseContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<MyCourseResponse>> GetEnrollmentsAsync(int userId)
        {
            var myCourse = await _eCourseContext.Enrollments.Include(b=>b.User).Include(x => x.Course).ThenInclude(y => y.Category).Where(a=>a.UserId == userId).Select(z => new MyCourseResponse
            {
                EnrollmentId = z.Id, 
                CourseId =z.CourseId,
                UserName = z.User.Username,
                Title = z.Course.Title,
                ThumbnailImage = z.Course.Thumbnail,
                Category = z.Course.Category.Name,
                ExpiredDate = z.ExpiredDate,
                Status = z.Status,
                Progress = z.Progress

            }).ToListAsync();
            return myCourse;
        }

        public async Task<bool> UpdateEnrollmentStatus(EnrollmentRequestDto enrollmentRequest)
        {
            var limitDate = await _eCourseContext.Courses.Where(x => x.Id == enrollmentRequest.CourseId).Select(x => x.LimitDay).FirstOrDefaultAsync();
            var enrollment = await _eCourseContext.Enrollments
                .Where(x => x.CourseId == enrollmentRequest.CourseId && x.UserId == enrollmentRequest.UserId)
                .FirstOrDefaultAsync();
            enrollment.Status = 1;
            enrollment.ExpiredDate = DateTime.Now.AddDays((double)limitDate / 2);

            _eCourseContext.Enrollments.Update(enrollment);
            await _eCourseContext.SaveChangesAsync();
            return true;
        }
    }
}
