using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IEnumerable<UserResponse> Users { get; set; } = new List<UserResponse>();
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public UserStatistics Statistics { get; set; } = new UserStatistics();

        [BindProperty]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedRole { get; set; } = string.Empty;

        [BindProperty]
        public string SelectedStatus { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                TempData["Success"] = "User deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting user: " + ex.Message;
            }

            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAddUserAsync(string name, string email, string phone, string bloodType, string address, int roleId, string password)
        {
            try
            {
                var userRequest = new UserRequest
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    BloodType = bloodType,
                    Address = address,
                    RoleId = roleId,
                    Password = password
                };

                await _userService.AddAsync(userRequest);
                TempData["Success"] = "User added successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error adding user: " + ex.Message;
            }

            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            var allUsers = await _userService.GetAllAsync();
            
            // Apply filters
            var filteredUsers = allUsers.AsEnumerable();
            
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filteredUsers = filteredUsers.Where(u => 
                    u.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true);
            }
            
            if (!string.IsNullOrEmpty(SelectedRole))
            {
                filteredUsers = filteredUsers.Where(u => 
                    u.Role.Name.Equals(SelectedRole, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(SelectedStatus))
            {
                var isActive = SelectedStatus.Equals("active", StringComparison.OrdinalIgnoreCase);
                filteredUsers = filteredUsers.Where(u => 
                    (u.IsDeleted != true) == isActive);
            }
            
            Users = filteredUsers.ToList();
            
            // Calculate statistics
            Statistics = new UserStatistics
            {
                TotalUsers = allUsers.Count(),
                DonorsCount = allUsers.Count(u => u.Role.Name.Equals("Donor", StringComparison.OrdinalIgnoreCase)),
                StaffCount = allUsers.Count(u => u.Role.Name.Equals("Staff", StringComparison.OrdinalIgnoreCase)),
                AdminsCount = allUsers.Count(u => u.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            };
            
            // Get distinct roles for filter dropdown
            Roles = allUsers.Select(u => u.Role.Name).Distinct().ToList();
        }

        public string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name)) return "??";
            
            var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return words.Length switch
            {
                1 => words[0].Substring(0, Math.Min(2, words[0].Length)).ToUpper(),
                _ => $"{words[0][0]}{words[^1][0]}".ToUpper()
            };
        }

        public string GetRoleColorClass(string roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "bg-red-100",
                "staff" => "bg-purple-100",
                "donor" => "bg-green-100",
                _ => "bg-blue-100"
            };
        }

        public string GetRoleTextColorClass(string roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "text-red-600",
                "staff" => "text-purple-600",
                "donor" => "text-green-600",
                _ => "text-blue-600"
            };
        }

        public string GetRoleBadgeClass(string roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "bg-red-100 text-red-800",
                "staff" => "bg-purple-100 text-purple-800",
                "donor" => "bg-green-100 text-green-800",
                _ => "bg-blue-100 text-blue-800"
            };
        }

        public string GetStatusBadgeClass(bool isActive)
        {
            return isActive ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800";
        }
    }

    public class UserStatistics
    {
        public int TotalUsers { get; set; }
        public int DonorsCount { get; set; }
        public int StaffCount { get; set; }
        public int AdminsCount { get; set; }
    }
}
