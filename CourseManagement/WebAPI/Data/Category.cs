using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
