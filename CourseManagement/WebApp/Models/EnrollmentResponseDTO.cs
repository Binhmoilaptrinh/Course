namespace WebApp.Models
{
    public class EnrollmentResponseDTO
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string UserName { get; set; }
        public string avatar { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public double Progress { get; set; }
        public int Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
