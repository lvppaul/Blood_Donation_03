using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class BlogPost
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int AuthorId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Category { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User Author { get; set; } = null!;
}
