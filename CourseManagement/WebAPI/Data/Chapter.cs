using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Chapter
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreateBy { get; set; } = null!;

    public string? UpdateBy { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual AspNetUser CreateByNavigation { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual AspNetUser? UpdateByNavigation { get; set; }
}
