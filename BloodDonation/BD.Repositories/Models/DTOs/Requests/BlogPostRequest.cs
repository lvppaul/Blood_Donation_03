using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class BlogPostRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Content is required")]
        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
        public string Content { get; set; } = null!;

        public int AuthorId { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        public bool? IsDocument { get; set; }
        public bool? IsPublished { get; set; }

        [StringLength(500, ErrorMessage = "Summary cannot exceed 500 characters")]
        public string? Summary { get; set; }

        [StringLength(255, ErrorMessage = "Tags cannot exceed 255 characters")]
        public string? Tags { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string? ImageUrl { get; set; }
    }
}
