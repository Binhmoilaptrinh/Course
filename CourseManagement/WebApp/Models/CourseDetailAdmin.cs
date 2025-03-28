namespace WebApp.Models
{
    public class CourseDetailAdmin
    {
        public string Thumbnail { get; set; }

        public string PreviewVideo { get; set; }
        public int Enrollments { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int LessonsCount { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }

        public string Description { get; set; }

        public int? LimitDay { get; set; }

    }
}
