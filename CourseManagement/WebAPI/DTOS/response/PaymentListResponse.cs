namespace WebAPI.DTOS.response
{
    public class PaymentListResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
    }
}
