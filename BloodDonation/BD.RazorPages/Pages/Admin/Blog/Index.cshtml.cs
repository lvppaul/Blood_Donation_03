using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public IndexModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public IEnumerable<BlogPostResponse> BlogPosts { get; set; } = new List<BlogPostResponse>();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? StatusFilter { get; set; }
        
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        
        public BlogStatistics Statistics { get; set; } = new BlogStatistics();

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // Get all blog posts (including unpublished and deleted for admin view)
                var allPosts = await _blogPostService.GetAllPostsAsync();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    allPosts = allPosts.Where(p => 
                        p.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Content.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Apply category filter
                if (!string.IsNullOrWhiteSpace(CategoryFilter) && CategoryFilter != "All")
                {
                    allPosts = allPosts.Where(p => 
                        string.Equals(p.Category, CategoryFilter, StringComparison.OrdinalIgnoreCase));
                }

                // Apply status filter
                if (!string.IsNullOrWhiteSpace(StatusFilter))
                {
                    switch (StatusFilter)
                    {
                        case "Published":
                            allPosts = allPosts.Where(p => p.IsPublished == true && p.IsDeleted == false);
                            break;
                        case "Draft":
                            allPosts = allPosts.Where(p => p.IsPublished == false && p.IsDeleted == false);
                            break;
                        case "Deleted":
                            allPosts = allPosts.Where(p => p.IsDeleted == true);
                            break;
                    }
                }

                // Order by creation date (newest first)
                BlogPosts = allPosts.OrderByDescending(p => p.CreatedAt).ToList();

                // Get unique categories for filter dropdown
                var allPostsList = await _blogPostService.GetAllPostsAsync();
                Categories = allPostsList
                    .Where(p => !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                // Calculate statistics
                Statistics = new BlogStatistics
                {
                    TotalPosts = allPostsList.Count(),
                    PublishedPosts = allPostsList.Count(p => p.IsPublished == true && p.IsDeleted == false),
                    DraftPosts = allPostsList.Count(p => p.IsPublished == false && p.IsDeleted == false),
                    DeletedPosts = allPostsList.Count(p => p.IsDeleted == true),
                    TotalCategories = Categories.Count(),
                    DocumentPosts = allPostsList.Count(p => p.IsDocument == true)
                };

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading blog posts: " + ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _blogPostService.DeletePostAsync(id);
                TempData["Success"] = "Blog post deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting blog post: " + ex.Message;
            }
            
            return RedirectToPage();
        }

        private bool IsAuthorized()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return !string.IsNullOrEmpty(userRole) && 
                   (userRole.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                    userRole.Equals("staff", StringComparison.OrdinalIgnoreCase));
        }
    }

    public class BlogStatistics
    {
        public int TotalPosts { get; set; }
        public int PublishedPosts { get; set; }
        public int DraftPosts { get; set; }
        public int DeletedPosts { get; set; }
        public int TotalCategories { get; set; }
        public int DocumentPosts { get; set; }
    }
}
