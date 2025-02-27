using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Comment
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string UserId { get; set; } = null!;

    public int? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Comment? ParentComment { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
