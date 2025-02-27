using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Finance
{
    public int Id { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public double Revenue { get; set; }

    public double Fee { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;
}
