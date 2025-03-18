namespace WebAPI.DTOS.response
{
    public class CoursePayment
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public int LessonCount { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
    }
}
