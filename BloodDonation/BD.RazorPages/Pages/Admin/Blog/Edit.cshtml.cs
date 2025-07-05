using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Updates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Blog
{
    public class EditModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public EditModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [BindProperty]
        public int PostId { get; set; }

        [BindProperty]
        public BlogPostUpdateRequest BlogPost { get; set; } = new BlogPostUpdateRequest();

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string AuthorName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            PostId = id;

            try
            {
                var blogPostResponse = await _blogPostService.GetPostByIdAsync(id);
                if (blogPostResponse == null)
                {
                    TempData["Error"] = "Blog post not found.";
                    return RedirectToPage("/Admin/Blog/Index");
                }

                // Map response to update request
                BlogPost = new BlogPostUpdateRequest
                {
                    Title = blogPostResponse.Title,
                    Content = blogPostResponse.Content,
                    Category = blogPostResponse.Category,
                    IsDocument = blogPostResponse.IsDocument,
                    IsPublished = blogPostResponse.IsPublished
                };

                CreatedAt = blogPostResponse.CreatedAt;
                UpdatedAt = blogPostResponse.UpdatedAt;
                AuthorName = blogPostResponse.AuthorName ?? "Unknown";

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading blog post: " + ex.Message;
                return RedirectToPage("/Admin/Blog/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            if (action == "delete")
            {
                return await HandleDeleteAsync();
            }

            if (!ModelState.IsValid)
            {
                // Reload additional data for the page
                await LoadPostInfoAsync();
                return Page();
            }

            try
            {
                // Set published status based on action
                if (action == "publish")
                {
                    BlogPost.IsPublished = true;
                }
                else if (action == "draft")
                {
                    BlogPost.IsPublished = false;
                }

                // Update the blog post
                var updatedPost = await _blogPostService.UpdatePostAsync(PostId, BlogPost);
                if (updatedPost == null)
                {
                    TempData["Error"] = "Blog post not found or could not be updated.";
                    return RedirectToPage("/Admin/Blog/Index");
                }

                // Set success message based on action
                if (BlogPost.IsPublished == true)
                {
                    TempData["Success"] = "Blog post updated and published successfully!";
                }
                else
                {
                    TempData["Success"] = "Blog post updated and saved as draft successfully!";
                }

                return RedirectToPage("/Admin/Blog/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating blog post: " + ex.Message;
                await LoadPostInfoAsync();
                return Page();
            }
        }

        private async Task<IActionResult> HandleDeleteAsync()
        {
            try
            {
                await _blogPostService.DeletePostAsync(PostId);
                TempData["Success"] = "Blog post deleted successfully!";
                return RedirectToPage("/Admin/Blog/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting blog post: " + ex.Message;
                await LoadPostInfoAsync();
                return Page();
            }
        }

        private async Task LoadPostInfoAsync()
        {
            try
            {
                var blogPostResponse = await _blogPostService.GetPostByIdAsync(PostId);
                if (blogPostResponse != null)
                {
                    CreatedAt = blogPostResponse.CreatedAt;
                    UpdatedAt = blogPostResponse.UpdatedAt;
                    AuthorName = blogPostResponse.AuthorName ?? "Unknown";
                }
            }
            catch
            {
                // Silently handle error for additional info loading
            }
        }

        private bool IsAuthorized()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return !string.IsNullOrEmpty(userRole) && 
                   (userRole.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                    userRole.Equals("staff", StringComparison.OrdinalIgnoreCase));
        }
    }
}
