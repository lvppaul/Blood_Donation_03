using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BD.RazorPages.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<SignupModel> _logger;

        public SignupModel(IUserService userService, IRoleService roleService, ILogger<SignupModel> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }

        [BindProperty]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters")]
        public string Phone { get; set; } = string.Empty;

        [BindProperty]
        public string BloodType { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }

        [BindProperty]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "You must agree to the terms and conditions")]
        public bool AgreeToTerms { get; set; }

        [BindProperty]
        public bool SubscribeToNewsletter { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public List<SelectListItem> BloodTypes { get; set; } = new List<SelectListItem>();

        public void OnGet()
        {
            // Check if user is already logged in
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                Response.Redirect("/");
                return;
            }

            LoadBloodTypes();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadBloodTypes();

            // Additional custom validation
            ValidateUserInput();

            // Check for existing email and phone
            await ValidateExistingUserAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userRequest = new UserRequest
                {
                    Name = Name.Trim(),
                    Email = Email.Trim().ToLower(),
                    Phone = Phone.Trim(),
                    BloodType = BloodType,
                    Address = string.IsNullOrWhiteSpace(Address) ? null : Address.Trim(),
                    RoleId = 3, // Default role: member (assuming 3 is member role based on login logic)
                    Password = Password
                };

                var createdUser = await _userService.AddAsync(userRequest);
                
                _logger.LogInformation("New user registered: {Email}", Email);
                
                // Auto-login the user after successful registration
                HttpContext.Session.SetString("UserId", createdUser.UserId.ToString());
                HttpContext.Session.SetString("UserName", createdUser.Name);
                HttpContext.Session.SetString("UserRole", "member");

                TempData["Success"] = "Registration successful! Welcome to BloodConnect.";
                
                // Redirect to profile page or dashboard
                return RedirectToPage("/Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for email: {Email}", Email);
                ErrorMessage = "An error occurred during registration. Please try again.";
                return Page();
            }
        }

        private void LoadBloodTypes()
        {
            BloodTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select Blood Type", Disabled = true },
                new SelectListItem { Value = "A+", Text = "A+" },
                new SelectListItem { Value = "A-", Text = "A-" },
                new SelectListItem { Value = "B+", Text = "B+" },
                new SelectListItem { Value = "B-", Text = "B-" },
                new SelectListItem { Value = "AB+", Text = "AB+" },
                new SelectListItem { Value = "AB-", Text = "AB-" },
                new SelectListItem { Value = "O+", Text = "O+" },
                new SelectListItem { Value = "O-", Text = "O-" }
            };
        }

        private void ValidateUserInput()
        {
            // Validate blood type
            var validBloodTypes = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            if (!string.IsNullOrEmpty(BloodType) && !validBloodTypes.Contains(BloodType))
            {
                ModelState.AddModelError(nameof(BloodType), "Please select a valid blood type");
            }

            // Validate name doesn't contain only numbers
            if (!string.IsNullOrEmpty(Name) && Name.Trim().All(char.IsDigit))
            {
                ModelState.AddModelError(nameof(Name), "Name cannot contain only numbers");
            }

            // Validate email format more strictly
            if (!string.IsNullOrEmpty(Email))
            {
                var emailTrimmed = Email.Trim();
                if (emailTrimmed.StartsWith(".") || emailTrimmed.EndsWith(".") || 
                    emailTrimmed.Contains("..") || emailTrimmed.Split('@').Length != 2)
                {
                    ModelState.AddModelError(nameof(Email), "Please enter a valid email address");
                }
            }

            // Validate phone number format
            if (!string.IsNullOrEmpty(Phone))
            {
                var phoneDigitsOnly = new string(Phone.Where(char.IsDigit).ToArray());
                if (phoneDigitsOnly.Length < 10 || phoneDigitsOnly.Length > 15)
                {
                    ModelState.AddModelError(nameof(Phone), "Phone number must contain 10-15 digits");
                }
            }

            // Additional password validation
            if (!string.IsNullOrEmpty(Password))
            {
                // Check for only whitespace characters as special chars
                if (Password.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    ModelState.AddModelError(nameof(Password), "Password must contain at least one special character (not just spaces)");
                }

                // Check for common weak passwords
                var weakPasswords = new[] { "123456", "password", "qwerty", "abc123", "password123" };
                if (weakPasswords.Any(weak => Password.ToLower().Contains(weak)))
                {
                    ModelState.AddModelError(nameof(Password), "Password is too common. Please choose a stronger password");
                }
            }
        }

        private async Task ValidateExistingUserAsync()
        {
            try
            {
                // Check if email already exists
                if (!string.IsNullOrEmpty(Email))
                {
                    var existingUserByEmail = await _userService.GetByEmailAsync(Email.Trim().ToLower());
                    if (existingUserByEmail != null)
                    {
                        ModelState.AddModelError(nameof(Email), "This email address is already registered. Please use a different email or try logging in.");
                    }
                }

                // Check if phone already exists
                if (!string.IsNullOrEmpty(Phone))
                {
                    var existingUserByPhone = await _userService.GetByPhoneAsync(Phone.Trim());
                    if (existingUserByPhone != null)
                    {
                        ModelState.AddModelError(nameof(Phone), "This phone number is already registered. Please use a different phone number.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating existing user during signup");
                // Log the error but don't fail validation - let the user try to submit
            }
        }
    }
}
