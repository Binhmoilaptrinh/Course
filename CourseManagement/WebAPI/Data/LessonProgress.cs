using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class LessonProgress
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string UserId { get; set; } = null!;

    public float ProgressPercentage { get; set; }

    public float? TimeSpent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public int? Passing { get; set; }

    public int? CountDoing { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
