using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Certificate
{
    public int Id { get; set; }

    public int EnrollmentId { get; set; }

    public DateTime IssueDate { get; set; }

    public string? CertificateNumber { get; set; }

    public string? CertificateUrl { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;
}
