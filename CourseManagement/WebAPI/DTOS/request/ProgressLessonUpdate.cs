﻿namespace WebAPI.DTOS.request
{
    public class ProgressLessonUpdate
    {
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public float ProgressPercentage { get; set; }
        public int Passing {  get; set; }
    }
}
