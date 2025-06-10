using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        public Task<IEnumerable<BlogPost>> GetAllPostsAsync();
        public Task<BlogPost> GetPostByIdAsync(int id);
        public Task<BlogPost> AddAsync(BlogPostRequest post);
        public Task<BlogPost> UpdatePostAsync(BlogPostRequest post);
        public Task DeletePostAsync(int id);
    }
}
