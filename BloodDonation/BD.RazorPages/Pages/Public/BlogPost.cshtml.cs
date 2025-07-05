using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Public
{
    public class BlogPostModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public BlogPostResponse? BlogPost { get; set; }
        public IEnumerable<BlogPostResponse> RelatedPosts { get; set; } = new List<BlogPostResponse>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Get the specific blog post
                BlogPost = await _blogPostService.GetPostByIdAsync(id);
                
                if (BlogPost == null || BlogPost.IsPublished != true || BlogPost.IsDeleted == true)
                {
                    TempData["Error"] = "Blog post not found or not available.";
                    return RedirectToPage("/Public/Blog");
                }

                // Get related posts (same category, excluding current post)
                var allPosts = await _blogPostService.GetAllPostsAsync();
                RelatedPosts = allPosts
                    .Where(p => p.PostId != id && 
                               p.IsPublished == true && 
                               p.IsDeleted == false &&
                               !string.IsNullOrEmpty(p.Category) &&
                               p.Category.Equals(BlogPost.Category, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .ToList();

                // If no related posts in same category, get latest posts
                if (!RelatedPosts.Any())
                {
                    RelatedPosts = allPosts
                        .Where(p => p.PostId != id && 
                                   p.IsPublished == true && 
                                   p.IsDeleted == false)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(3)
                        .ToList();
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading blog post: " + ex.Message;
                return RedirectToPage("/Public/Blog");
            }
        }
    }
}
