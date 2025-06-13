using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloodDonationDbContext _context;
        public BlogPostRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<BlogPost> AddBlogAsync(BlogPost post)
        {
            await _context.BlogPosts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task DeletePostAsync(BlogPost blogPost)
        {
            blogPost.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
        {
            return await _context.BlogPosts.Where(bp => bp.IsDeleted != true).ToListAsync();
        }

        public async Task<BlogPost?> GetPostByIdAsync(int id)
        {
            return await _context.BlogPosts
                .FirstOrDefaultAsync(bp => bp.PostId == id && bp.IsDeleted != true);
        }

        public async Task<BlogPost> UpdatePostAsync(BlogPost post)
        {
            _context.BlogPosts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }
    }
}
