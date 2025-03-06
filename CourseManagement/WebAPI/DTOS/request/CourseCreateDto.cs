namespace WebAPI.DTOS.request
{
    public class CourseRequestDto
    {
        public string? Title { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string Description { get; set; }

        //co the null
        public string Status { get; set; }
        public IFormFile? PreviewVideo { get; set; } 
        public int? LimitDay { get; set; }
    }
}
