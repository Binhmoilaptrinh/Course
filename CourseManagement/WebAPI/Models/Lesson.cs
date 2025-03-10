using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ChapterId { get; set; }

        public string Type { get; set; }

        public string? VideoUrl { get; set; }
        public string Status { get; set; }

        public string? Content { get; set; }

        public float? Duration { get; set; }

        public float? Passing { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreateBy { get; set; }

        public int? UpdateBy { get; set; }

        public User Creator { get; set; }
        public User Updater { get; set; }
        public Chapter Chapter { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
