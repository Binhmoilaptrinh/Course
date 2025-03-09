using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollCourse(EnrollmentRequestDto enrollment);
        Task<int> CheckStatusEnrollment(EnrollmentRequestDto enrollment);
        Task<bool> UpdateEnrollmentStatus(EnrollmentRequestDto enrollmentRequest);
        Task<List<MyCourseResponse>> GetEnrollmentsAsync(int userId);
    }
}
