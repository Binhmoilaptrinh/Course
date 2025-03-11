using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Question
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public string QuestionText { get; set; }

        public Lesson Lesson { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
