using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.request;
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

        public async Task<bool> EnrollCourse(EnrollmentRequestDto enrollmentRequest)
        {
            var limitedDate = await _eCourseContext.Courses.Where(x => x.Id == enrollmentRequest.CourseId).Select(x=>x.LimitDay).FirstOrDefaultAsync();
            var enrollment = new Enrollment()
            {
                UserId = enrollmentRequest.UserId,
                CourseId = enrollmentRequest.CourseId,
                Progress = 0,
                EnrollmentDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddDays((double)limitedDate)
            };
            _eCourseContext.Enrollments.Add(enrollment);
            await _eCourseContext.SaveChangesAsync();
            return true;
        }
    }
}
