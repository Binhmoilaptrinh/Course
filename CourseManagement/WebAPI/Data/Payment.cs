using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Payment
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string TransactionId { get; set; } = null!;

    public bool IsSuccessful { get; set; }

    public int CourseId { get; set; }

    public string UserId { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public int Status { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
