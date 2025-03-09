using WebAPI.DTOS.request;
using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollCourse(EnrollmentRequestDto enrollment);
    }
}
