using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOS.request
{
    public class CategoryRequestDto
    {
        [MaxLength(20)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Status { get; set; }
    }
}
