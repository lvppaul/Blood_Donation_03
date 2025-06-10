using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class BlogPostService : IBlogPostService
    {
        public Task<BlogPost> AddAsync(BlogPostRequest post)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllPostsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> GetPostByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> UpdatePostAsync(int id, BlogPostRequest post)
        {
            throw new NotImplementedException();
        }
    }
}
