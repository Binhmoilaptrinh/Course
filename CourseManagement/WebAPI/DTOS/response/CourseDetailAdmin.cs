namespace WebAPI.DTOS.response
{
    public class CourseDetailAdmin
    {
        public string Thumbnail {  get; set; }
        public int Enrollments { get; set; }
        public int CourseId { get; set; }   
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public int LessonsCount { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }

    }
}
