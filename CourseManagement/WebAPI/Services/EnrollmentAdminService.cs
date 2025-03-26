using Microsoft.EntityFrameworkCore;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class EnrollmentAdminService : IEnrollmentAdminService
    {
        private readonly ECourseContext _eCourseContext;
        public EnrollmentAdminService(ECourseContext eCourseContext)
        {
            _eCourseContext = eCourseContext;
        }

        public int AmountEnroll(int courseId)
        {
            return _eCourseContext.Enrollments.Where(er => er.CourseId == courseId).Count();
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentList()
        {
            return await _eCourseContext.Enrollments.ToListAsync();
        }

        public async Task<IEnumerable<EnrollmentResponseDTO>> GetEnrollmentListByCourseId(int courseId)
        {
            return await _eCourseContext.Enrollments.Where(x => x.CourseId == courseId).Select(x => new EnrollmentResponseDTO
            {
                EnrollmentId = x.Id,
                CourseId = x.CourseId,
                UserName = x.User.Username,
                avatar = x.User.Avatar ?? "Empty Avatar",
                EnrollmentDate = x.EnrollmentDate,
                Progress = x.Progress,
                Status = x.Status,
                ExpiredDate = x.ExpiredDate
            }).ToListAsync();
        }

        public async Task<IEnumerable<Enrollment>> GetListCustomerEnrollmentByUserId(int userId)
        {
            return await _eCourseContext.Enrollments.Include(c => c.Course).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
