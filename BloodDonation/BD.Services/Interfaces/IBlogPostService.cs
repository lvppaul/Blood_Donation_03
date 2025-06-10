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
        public Task<IEnumerable<BlogPost>> GetAllPostsAsync();
        public Task<BlogPost> GetPostByIdAsync(int id);
        public Task<BlogPost> AddAsync(BlogPostRequest post);
        public Task<BlogPost> UpdatePostAsync(BlogPostRequest post);
        public Task DeletePostAsync(int id);
    }
}
