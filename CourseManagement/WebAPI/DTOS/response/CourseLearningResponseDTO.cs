namespace WebAPI.DTOS.response
{
    public class CourseLearningResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public List<ChapterLearningResponse> Chapters { get; set; }
    }
}
