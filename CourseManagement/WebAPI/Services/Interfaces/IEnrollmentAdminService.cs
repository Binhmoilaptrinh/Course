using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IEnrollmentAdminService
    {
        Task<IEnumerable<Enrollment>> GetListCustomerEnrollmentByUserId(int userId);
        //Task<PaginatedResult<Enrollment>> GetEnrollmentsAsync(string searchTerm, string sortField, bool isAscending, int page, int pageSize, string userId);
        Task<IEnumerable<Enrollment>> GetEnrollmentList();
        int AmountEnroll(int courseId);
        //Task<List<EnrollmentDto>> EnrollmentListByCourseId(int courseId);
        Task<IEnumerable<EnrollmentResponseDTO>> GetEnrollmentListByCourseId(int courseId);
    }
}
