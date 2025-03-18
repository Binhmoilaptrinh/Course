using System.Text.Json.Serialization;

namespace WebAPI.DTOS.request
{
    public class PaymentRequest
    {
        public int CourseId {  get; set; }
        public long OrderCode { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get; set; }
    }

    public class ConfirmRequest
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
