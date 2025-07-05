using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Blog
{
    public class CreateModel : PageModel
    {
        private readonly IBlogPostService _blogPostService;

        public CreateModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [BindProperty]
        public BlogPostRequest BlogPost { get; set; } = new BlogPostRequest();

        public IActionResult OnGet()
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            // Check if user is authenticated and has Admin or Staff role
            if (!IsAuthorized())
            {
                return RedirectToPage("/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Set the author ID from session
                var userId = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int authorId))
                {
                    TempData["Error"] = "Unable to identify the author. Please log in again.";
                    return RedirectToPage("/Login");
                }

                BlogPost.AuthorId = authorId;

                // Set published status based on action
                if (action == "publish")
                {
                    BlogPost.IsPublished = true;
                }
                else if (action == "draft")
                {
                    BlogPost.IsPublished = false;
                }

                // Create the blog post using the service
                await _blogPostService.AddAsync(BlogPost);

                // Set success message based on action
                if (BlogPost.IsPublished == true)
                {
                    TempData["Success"] = "Blog post published successfully!";
                }
                else
                {
                    TempData["Success"] = "Blog post saved as draft successfully!";
                }

                return RedirectToPage("/Admin/Blog/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating blog post: " + ex.Message;
                return Page();
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
