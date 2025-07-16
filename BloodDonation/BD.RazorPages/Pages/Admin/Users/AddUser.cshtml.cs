using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Users
{
    public class AddUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AddUserModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
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
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 20 characters")]
        public string Phone { get; set; } = string.Empty;

        [BindProperty]
        public string BloodType { get; set; } = string.Empty;

        [BindProperty]
        public string? Address { get; set; }

        [BindProperty]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
        public int RoleId { get; set; }

        [BindProperty]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        public IEnumerable<RoleResponse> AllRoles { get; set; } = new List<RoleResponse>();

        public async Task OnGetAsync()
        {
            await LoadRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRolesAsync();

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
                    RoleId = RoleId,
                    Password = Password
                };

                await _userService.AddAsync(userRequest);
                TempData["Success"] = "User added successfully!";
                
                // Redirect to Users page after successful addition
                return RedirectToPage("/Admin/Users/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding user: " + ex.Message;
                return Page();
            }
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                AllRoles = await _roleService.GetAllRolesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading roles: {ex.Message}");
                AllRoles = new List<RoleResponse>();
                TempData["Error"] = "Error loading roles. Please try again.";
            }
        }

        private void ValidateUserInput()
        {
            // Validate blood type
            var validBloodTypes = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            if (!string.IsNullOrEmpty(BloodType) && !validBloodTypes.Contains(BloodType))
            {
                ModelState.AddModelError(nameof(BloodType), "Please select a valid blood type");
            }

            // Validate role exists
            if (RoleId > 0 && AllRoles.Any())
            {
                var roleExists = AllRoles.Any(r => r.RoleId == RoleId);
                if (!roleExists)
                {
                    ModelState.AddModelError(nameof(RoleId), "Please select a valid role");
                }
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

            // Additional password validation (beyond regex)
            if (!string.IsNullOrEmpty(Password))
            {
                // Check for only whitespace characters as special chars
                if (Password.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    ModelState.AddModelError(nameof(Password), "Password must contain at least one special character (not just spaces)");
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
                        ModelState.AddModelError(nameof(Email), "This email address is already registered");
                    }
                }

                // Check if phone already exists
                if (!string.IsNullOrEmpty(Phone))
                {
                    var existingUserByPhone = await _userService.GetByPhoneAsync(Phone.Trim());
                    if (existingUserByPhone != null)
                    {
                        ModelState.AddModelError(nameof(Phone), "This phone number is already registered");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail validation - let the user try to submit
                Console.WriteLine($"Error validating existing user: {ex.Message}");
            }
        }
    }
}
