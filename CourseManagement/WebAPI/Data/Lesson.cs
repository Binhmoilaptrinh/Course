using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Lesson
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ChapterId { get; set; }

    public string Type { get; set; } = null!;

    public string? VideoUrl { get; set; }

    public string Status { get; set; } = null!;

    public string? Content { get; set; }

    public float? Duration { get; set; }

    public float? Passing { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreateBy { get; set; } = null!;

    public string? UpdateBy { get; set; }

    public virtual Chapter Chapter { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual AspNetUser CreateByNavigation { get; set; } = null!;

    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual AspNetUser? UpdateByNavigation { get; set; }
}
