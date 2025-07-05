using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly BloodDonationDbContext _context;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(BloodDonationDbContext context, ILogger<LoginModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            // Check if user is already logged in
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                Response.Redirect("/");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Check database for real users
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == Email && u.IsDeleted != true && u.Password == Password);

                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                    return Page();
                }

                // For demo purposes, we'll accept any password for database users
                // In a real application, you would hash and verify the password
                var roleId = user.RoleId;
                var roleName = GetRoleName(roleId);

                SetUserSession(user.UserId.ToString(), user.Name, roleName);

                _logger.LogInformation($"User {Email} logged in successfully with role {roleName}");

                // Redirect based on role
                if (roleName == "admin" || roleName == "staff")
                {
                    return RedirectToPage("/Admin/Dashboard");
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt for {Email}", Email);
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "admin",
                2 => "staff",
                3 => "member",
                _ => "guest"
            };
        }
        private void SetUserSession(string userId, string name, string role)
        {
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("UserName", name);
            HttpContext.Session.SetString("UserRole", role);

            if (RememberMe)
            {
                // Set longer session timeout for remember me
                HttpContext.Session.SetString("RememberMe", "true");
            }
        }
    }
}
