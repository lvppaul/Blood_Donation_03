using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Get user info before clearing session
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            // Clear all session data
            HttpContext.Session.Clear();

            // Log the logout
            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation($"User {userName} (ID: {userId}) logged out successfully");
            }

            // Show logout confirmation page
            return Page();
        }

        public IActionResult OnPost()
        {
            // Handle POST request (if logout is triggered via form)
            return OnGet();
        }
    }
}
