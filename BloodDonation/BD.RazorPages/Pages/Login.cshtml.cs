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
        private readonly IDonorAvailabilityRepository _donorAvailabilityRepository;

        public LoginModel(BloodDonationDbContext context, ILogger<LoginModel> logger, IDonorAvailabilityRepository donorAvailabilityRepository)
        {
            _context = context;
            _logger = logger;
            _donorAvailabilityRepository = donorAvailabilityRepository;
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

                // Update donor availability status if recovery period has passed
                await UpdateDonorStatusIfRecoveryPeriodPassedAsync(user.UserId.ToString());

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

        /// <summary>
        /// Updates donor availability status from Recovery Period to Available if recovery period has passed
        /// This method is called when user logs in successfully
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns></returns>
        private async Task UpdateDonorStatusIfRecoveryPeriodPassedAsync(string userId)
        {
            if (userId.StartsWith("demo-"))
            {
                return; // Skip for demo accounts
            }

            if (int.TryParse(userId, out int userIdInt))
            {
                try
                {
                    var latestDonorAvailability = await _donorAvailabilityRepository.GetLatestDonorAvailabilityByUserIdAsync(userIdInt);
                    if (latestDonorAvailability != null && 
                        latestDonorAvailability.RecoveryReminderDate.HasValue &&
                        latestDonorAvailability.StatusDonorId == 3) // Recovery Period status
                    {
                        var recoveryDate = latestDonorAvailability.RecoveryReminderDate.Value.ToDateTime(TimeOnly.MinValue);
                        
                        // Auto-update status if recovery period has passed
                        if (DateTime.Today >= recoveryDate.Date)
                        {
                            latestDonorAvailability.StatusDonorId = 1; // Change to Available
                            await _donorAvailabilityRepository.UpdateDonorAvailabilityAsync(latestDonorAvailability);
                            _logger.LogInformation("Auto-updated donor availability status to Available for user {UserId}", userIdInt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating donor availability status for user {UserId}", userIdInt);
                }
            }
        }
    }
}
