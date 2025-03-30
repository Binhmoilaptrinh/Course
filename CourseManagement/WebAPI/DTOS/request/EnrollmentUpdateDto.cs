namespace WebAPI.DTOS.request
{
    public class EnrollmentUpdateDto
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public double Progress { get; set; }
    }
}
