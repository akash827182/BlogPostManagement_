using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Shared.DTO.MainData
{
    public class BlogPostDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is Required")]
        public required string Title { get; set; }
        public required string Author { get; set; }
        [Required(ErrorMessage = "Content is Required")]
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; }

        public int UserId { get; set; }



    }
}
