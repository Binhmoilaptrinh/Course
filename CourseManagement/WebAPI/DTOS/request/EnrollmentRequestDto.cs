using MimeKit.Tnef;

namespace WebAPI.DTOS.request
{
    public class EnrollmentRequestDto
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
