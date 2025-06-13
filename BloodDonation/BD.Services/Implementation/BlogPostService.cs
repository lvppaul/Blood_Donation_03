using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogPostService(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPostResponse> AddAsync(BlogPostRequest post)
        {
            var blogPost = BlogPostMapper.ToEntity(post);
            var createdPost = await _blogPostRepository.AddBlogAsync(blogPost);

            return BlogPostMapper.ToResponse(createdPost);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _blogPostRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                throw new Exception("Blog post not found");
            }
            
            await _blogPostRepository.DeletePostAsync(post);
        }

        public async Task<IEnumerable<BlogPostResponse>> GetAllPostsAsync()
        {
            var listPost = await _blogPostRepository.GetAllPostsAsync();

            return listPost.Select(p => BlogPostMapper.ToResponse(p));
        }

        public async Task<BlogPostResponse?> GetPostByIdAsync(int id)
        {
            var existingPost = await _blogPostRepository.GetPostByIdAsync(id);
            if (existingPost == null)
            {
                return null;
            }
            return BlogPostMapper.ToResponse(existingPost);
        }

        public async Task<BlogPostResponse?> UpdatePostAsync(BlogPostRequest post)
        {
            var existingPost = BlogPostMapper.ToEntity(post);
            var updatedPost = await _blogPostRepository.UpdatePostAsync(existingPost);
            if (updatedPost == null)
            {
                return null; // or throw an exception if you prefer
            }
            return BlogPostMapper.ToResponse(updatedPost);
        }
    }
}
