using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPost>> GetAllPostsAsync();
        Task<BlogPost> GetPostByIdAsync(int id);
        Task<BlogPost> AddAsync(BlogPostRequest post);
        Task<BlogPost> UpdatePostAsync(int id, BlogPostRequest post);
        Task DeletePostAsync(int id);
    }
}
