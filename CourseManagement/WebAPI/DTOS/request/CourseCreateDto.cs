using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOS.request
{
    public class CourseRequestDto
    {
        public int UserId { get; set; }

        public string Title { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string Description { get; set; }

        [RegularExpression("^(pending|online)$", ErrorMessage = "Status must be 'pending' or 'online'.")]
        public string Status { get; set; }
        public IFormFile? PreviewVideo { get; set; } 
        public int LimitDay { get; set; }
    }
}
