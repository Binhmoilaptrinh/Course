using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Discount
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public decimal DiscountPer { get; set; }

    public int MaxUses { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string CreateBy { get; set; } = null!;

    public string? UpdateBy { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;
}
