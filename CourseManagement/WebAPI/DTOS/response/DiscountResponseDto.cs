namespace WebAPI.DTOS.response
{
    public class DiscountResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountPer { get; set; }
        public int MaxUses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; } // Lấy tên khóa học từ bảng Course
    }
}
