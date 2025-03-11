using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebAPI.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public User Creator { get; set; }
        public User Updater { get; set; }
        public Course Course { get; set; }
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    }
}
