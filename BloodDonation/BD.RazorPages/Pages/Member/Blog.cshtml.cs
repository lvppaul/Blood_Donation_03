using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Member
{
    public class BlogModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public BlogModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public IEnumerable<BlogPostResponse> BlogPosts { get; set; } = new List<BlogPostResponse>();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }
        
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        
        public BlogStatistics Statistics { get; set; } = new BlogStatistics();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get all published blog posts
                var allPosts = await _blogPostService.GetAllPostsAsync();
                var publishedPosts = allPosts.Where(p => p.IsPublished == true && p.IsDeleted == false);

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    publishedPosts = publishedPosts.Where(p => 
                        p.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Content.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Apply category filter
                if (!string.IsNullOrWhiteSpace(CategoryFilter) && CategoryFilter != "All")
                {
                    publishedPosts = publishedPosts.Where(p => 
                        string.Equals(p.Category, CategoryFilter, StringComparison.OrdinalIgnoreCase));
                }

                // Order by creation date (newest first)
                BlogPosts = publishedPosts.OrderByDescending(p => p.CreatedAt).ToList();

                // Get unique categories for filter dropdown
                Categories = allPosts
                    .Where(p => p.IsPublished == true && p.IsDeleted == false && !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                // Calculate statistics
                Statistics = new BlogStatistics
                {
                    TotalPosts = publishedPosts.Count(),
                    TotalCategories = Categories.Count(),
                    RecentPosts = publishedPosts.Where(p => p.CreatedAt >= DateTime.Now.AddDays(-7)).Count(),
                    DocumentPosts = publishedPosts.Where(p => p.IsDocument == true).Count()
                };

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading blog posts: " + ex.Message;
                return Page();
            }
        }
    }

    public class BlogStatistics
    {
        public int TotalPosts { get; set; }
        public int TotalCategories { get; set; }
        public int RecentPosts { get; set; }
        public int DocumentPosts { get; set; }
    }
}
