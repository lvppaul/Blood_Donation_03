using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Updates
{
    public class BlogPostUpdateRequest
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Category { get; set; }
        public bool? IsDocument { get; set; }
        public bool? IsPublished { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
