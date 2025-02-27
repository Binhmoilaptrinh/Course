using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Enrollment
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public double Progress { get; set; }

    public int Status { get; set; }

    public DateTime? ExpiredDate { get; set; }

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual Course Course { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
