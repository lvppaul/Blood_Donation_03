using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly BloodDonationDbContext _context;
        private readonly ILogger<ProfileModel> _logger;

        public ProfileModel(IUserRepository userRepository, BloodDonationDbContext context, ILogger<ProfileModel> logger)
        {
            _userRepository = userRepository;
            _context = context;
            _logger = logger;
        }        // Display properties
        public string UserId { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public bool IsEditMode { get; set; } = false;
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Editable properties with validation
        [BindProperty]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string UserName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Blood type is required")]
        public string BloodType { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? NewPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }

        public async Task<IActionResult> OnGetAsync(bool edit = false)
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            IsEditMode = edit;
            await LoadUserDataAsync(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            UserId = userId;
            UserRole = HttpContext.Session.GetString("UserRole") ?? "guest";

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the errors below.";
                IsEditMode = true;
                return Page();
            }

            try
            {
                // For demo accounts, just update session
                if (userId.StartsWith("demo-"))
                {
                    HttpContext.Session.SetString("UserName", UserName);
                    SuccessMessage = "Demo profile updated successfully! (Note: Changes are session-only for demo accounts)";
                    _logger.LogInformation($"Demo user {userId} updated profile");
                }
                else
                {
                    // Update real user in database
                    if (int.TryParse(userId, out int userIdInt))
                    {
                        var user = await _userRepository.GetByIdAsync(userIdInt);
                        if (user != null)
                        {
                            // Update user properties
                            user.Name = UserName;
                            user.Email = Email;
                            user.Phone = Phone;
                            user.BloodType = BloodType;
                            user.Address = Address;

                            // Update password if provided
                            if (!string.IsNullOrEmpty(NewPassword))
                            {
                                // In a real application, you would hash the password
                                user.Password = NewPassword;
                            }

                            await _userRepository.UpdateAsync(user);

                            // Update session with new name
                            HttpContext.Session.SetString("UserName", UserName);

                            SuccessMessage = "Profile updated successfully!";
                            _logger.LogInformation($"User {userId} updated profile");
                        }
                        else
                        {
                            ErrorMessage = "User not found.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Invalid user ID.";
                    }
                }

                IsEditMode = false;
                // Clear password fields after update
                NewPassword = null;
                ConfirmPassword = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile for user {UserId}", userId);
                ErrorMessage = "An error occurred while updating your profile. Please try again.";
                IsEditMode = true;
            }

            return Page();
        }

        private async Task LoadUserDataAsync(string userId)
        {
            UserId = userId;
            UserRole = HttpContext.Session.GetString("UserRole") ?? "guest";            // Load demo account data
            if (userId.StartsWith("demo-"))
            {
                UserName = HttpContext.Session.GetString("UserName") ?? "Demo User";
                Email = GetDemoEmail(userId);
                Phone = GetDemoPhone(userId);
                BloodType = GetDemoBloodType(userId);
                Address = GetDemoAddress(userId);
                DateCreated = DateTime.Now.AddDays(-30); // Demo accounts created 30 days ago
            }
            else
            {
                // Load real user data from database
                if (int.TryParse(userId, out int userIdInt))
                {
                    var user = await _userRepository.GetByIdAsync(userIdInt);
                    if (user != null)
                    {
                        UserName = user.Name;
                        Email = user.Email ?? "";
                        Phone = user.Phone;
                        BloodType = user.BloodType;
                        Address = user.Address;
                        DateCreated = user.CreatedAt ?? DateTime.Now.AddDays(-7); // Default to 7 days ago if not set
                    }
                    else
                    {
                        UserName = "Unknown User";
                        Email = "";
                        BloodType = "Unknown";
                        DateCreated = DateTime.Now;
                    }
                }
            }
        }

        private string GetDemoEmail(string userId)
        {
            return userId switch
            {
                "demo-guest" => "guest@bloodconnect.com",
                "demo-member" => "member@bloodconnect.com",
                "demo-staff" => "staff@bloodconnect.com",
                "demo-admin" => "admin@bloodconnect.com",
                _ => "demo@bloodconnect.com"
            };
        }

        private string GetDemoPhone(string userId)
        {
            return userId switch
            {
                "demo-guest" => "(555) 000-0001",
                "demo-member" => "(555) 000-0002",
                "demo-staff" => "(555) 000-0003",
                "demo-admin" => "(555) 000-0004",
                _ => "(555) 000-0000"
            };
        }

        private string GetDemoBloodType(string userId)
        {
            return userId switch
            {
                "demo-guest" => "O+",
                "demo-member" => "A+",
                "demo-staff" => "B+",
                "demo-admin" => "AB+",
                _ => "O+"
            };
        }

        private string GetDemoAddress(string userId)
        {
            return userId switch
            {
                "demo-guest" => "123 Guest Street, Demo City",
                "demo-member" => "456 Member Avenue, Demo City",
                "demo-staff" => "789 Staff Boulevard, Demo City",
                "demo-admin" => "321 Admin Plaza, Demo City",
                _ => "Demo Address"
            };
        }

        public List<string> GetRolePermissions()
        {
            return UserRole.ToLower() switch
            {
                "admin" => new List<string>
                {
                    "Full system administration access",
                    "Manage all users and roles",
                    "Access admin dashboard",
                    "Manage blood inventory",
                    "View all reports and analytics",
                    "System configuration settings"
                },
                "staff" => new List<string>
                {
                    "Access staff dashboard",
                    "View public content",
                    "Search for blood donors",
                    "Manage emergency requests",
                    "Send donation reminders",
                    "View blood inventory status",
                    "Generate basic reports"
                },
                "member" => new List<string>
                {
                    "Manage donor profile",
                    "View public content",
                    "Update availability status",
                    "Search for blood requests",
                    "View emergency requests",
                    "Receive notifications",
                    "Access donation history",
                    "Register to donate blood"
                },
                _ => new List<string>
                {
                    "View public content",
                    "Read blog articles",
                    "Access basic information",
                    "Register as donor"
                }
            };
        }
    }
}
