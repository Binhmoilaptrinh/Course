namespace WebAPI.DTOS.request
{
    public class DiscountRequestDto
    {
        public decimal DiscountPer { get; set; }
        public int MaxUses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CourseId { get; set; }
    }
}
