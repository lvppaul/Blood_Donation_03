using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Pages.Admin.Users
{
    public class EditUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public EditUserModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
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
        public string? NewPassword { get; set; }

        [BindProperty]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public IEnumerable<RoleResponse> AllRoles { get; set; } = new List<RoleResponse>();
        
        // Store original values for validation - only phone now since email is not editable
        public string OriginalPhone { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                await LoadRolesAsync();
                
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToPage("/Admin/Users/Index");
                }

                // Populate form with user data
                UserId = user.UserId;
                Name = user.Name;
                Email = user.Email ?? string.Empty;
                Phone = user.Phone ?? string.Empty;
                BloodType = user.BloodType ?? string.Empty;
                Address = user.Address;
                RoleId = user.Role?.RoleId ?? 0;

                // Store original values for validation - only phone now since email is not editable
                OriginalPhone = Phone;
                
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading user: " + ex.Message;
                return RedirectToPage("/Admin/Users/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRolesAsync();

            // Store original values for validation - only phone now since email is not editable
            try
            {
                var existingUser = await _userService.GetByIdAsync(UserId);
                if (existingUser != null)
                {
                    OriginalPhone = existingUser.Phone ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error retrieving user data: " + ex.Message;
                return Page();
            }

            // Additional custom validation
            ValidateUserInput();

            // Check for existing phone (excluding current user) - email is no longer editable
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
                    RoleId = RoleId
                };

                // Only set password if provided
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    userRequest.Password = NewPassword;
                }

                await _userService.UpdateAsync(UserId, userRequest);

                TempData["Success"] = "User updated successfully!";
                return RedirectToPage("/Admin/Users/Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating user: " + ex.Message;
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

            // Validate phone number format
            if (!string.IsNullOrEmpty(Phone))
            {
                var phoneDigitsOnly = new string(Phone.Where(char.IsDigit).ToArray());
                if (phoneDigitsOnly.Length < 10 || phoneDigitsOnly.Length > 15)
                {
                    ModelState.AddModelError(nameof(Phone), "Phone number must contain 10-15 digits");
                }
            }

            // Additional password validation (only if password is provided)
            if (!string.IsNullOrEmpty(NewPassword))
            {
                // Check for only whitespace characters as special chars
                if (NewPassword.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    ModelState.AddModelError(nameof(NewPassword), "Password must contain at least one special character (not just spaces)");
                }
            }

            // Validate password confirmation only if new password is provided
            if (!string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmPassword))
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "Please confirm your new password");
            }
        }

        private async Task ValidateExistingUserAsync()
        {
            try
            {
                // Check if phone already exists (excluding current user) - email validation removed since it's no longer editable
                if (!string.IsNullOrEmpty(Phone) && !Phone.Equals(OriginalPhone, StringComparison.OrdinalIgnoreCase))
                {
                    var existingUserByPhone = await _userService.GetByPhoneAsync(Phone.Trim());
                    if (existingUserByPhone != null && existingUserByPhone.UserId != UserId)
                    {
                        ModelState.AddModelError(nameof(Phone), "This phone number is already registered by another user");
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
