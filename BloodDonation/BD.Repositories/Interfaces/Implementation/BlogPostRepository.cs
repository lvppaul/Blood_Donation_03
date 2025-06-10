using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloodDonationDbContext _context;
        public BlogPostRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<BlogPost> AddAsync(BlogPostRequest post)
        {
            var entity = BlogPostMapper.ToEntity(post);

            await _context.BlogPosts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
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

        public Task<BlogPost> UpdatePostAsync(BlogPostRequest post)
        {
            throw new NotImplementedException();
        }
    }
}
