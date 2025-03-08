﻿namespace WebAPI.DTOS.response
{
    public class LessonProgressResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public float ProgressPercentage { get; set; }
        public string? Status { get; set; }
        public int? Passing { get; set; }
        public int? CountDoing { get; set; }
    }
}
