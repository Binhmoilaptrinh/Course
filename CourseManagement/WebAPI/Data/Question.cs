using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Question
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string QuestionText { get; set; } = null!;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Lesson Lesson { get; set; } = null!;
}
