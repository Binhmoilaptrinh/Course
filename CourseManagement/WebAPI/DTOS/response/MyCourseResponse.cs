using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebAPI.DTOS.response
{
    public class MyCourseResponse
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string? UserName { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? ThumbnailImage { get; set; }
        public double Progress { get; set; }
        public int Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
