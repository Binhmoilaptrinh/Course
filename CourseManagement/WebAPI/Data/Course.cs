using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Course
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int CategoryId { get; set; }

    public double Price { get; set; }

    public string Thumbnail { get; set; } = null!;

    public string Description { get; set; } = null!;

    public TimeOnly? Duration { get; set; }

    public string Status { get; set; } = null!;

    public string PreviewVideo { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreateBy { get; set; } = null!;

    public string? UpdateBy { get; set; }

    public int? LimitDay { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    public virtual AspNetUser CreateByNavigation { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual AspNetUser? UpdateByNavigation { get; set; }
}
