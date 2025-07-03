using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<UserResponse> Users { get; set; } = new List<UserResponse>();
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public UserStatistics Statistics { get; set; } = new UserStatistics();

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalUsers { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalUsers / PageSize);

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SelectedRole { get; set; } = string.Empty;

        [BindProperty]
        public int SelectedPageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            // Properties are automatically bound from query string due to SupportsGet = true
            await LoadDataAsync();
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

        private async Task LoadDataAsync()
        {
            try
            {
                // Get all data
                var allUsers = await SafeGetDataAsync(() => _userService.GetAllAsync()) ?? Enumerable.Empty<UserResponse>();

                // Apply filters to users
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
                
                // Calculate total count before pagination
                TotalUsers = filteredUsers.Count();
                
                // Apply pagination with consistent ordering
                var skip = (CurrentPage - 1) * PageSize;
                
                Users = filteredUsers
                    .OrderBy(u => u.UserId) // Ensure consistent ordering
                    .Skip(skip)
                    .Take(PageSize)
                    .ToList();
                
                // Calculate statistics using the same logic as Dashboard with null checks
                Statistics = new UserStatistics
                {
                    // Total users count
                    TotalUsers = allUsers?.Count() ?? 0,
                    // Member count - users with Member role
                    MemberCount = allUsers?.Count(u => u?.Role?.Name?.Equals("Member", StringComparison.OrdinalIgnoreCase) == true) ?? 0,
                    // Staff count - users with Staff role
                    StaffCount = allUsers?.Count(u => u?.Role?.Name?.Equals("Staff", StringComparison.OrdinalIgnoreCase) == true) ?? 0,
                    // Admins count - users with Admin role
                    AdminsCount = allUsers?.Count(u => u?.Role?.Name?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true) ?? 0
                };
                
                // Get distinct roles for filter dropdown with null checks
                Roles = allUsers?.Where(u => u?.Role?.Name != null).Select(u => u.Role.Name).Distinct().ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                // Handle errors gracefully with logging
                Console.WriteLine($"Error in Users LoadDataAsync: {ex.Message}");
                
                // Set default values on error
                Users = new List<UserResponse>();
                Statistics = new UserStatistics();
                Roles = new List<string>();
            }
        }

        private async Task<IEnumerable<T>?> SafeGetDataAsync<T>(Func<Task<IEnumerable<T>>> dataFunction)
        {
            try
            {
                return await dataFunction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data: {ex.Message}");
                return null;
            }
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
                "member" => "bg-green-100",
                _ => "bg-blue-100"
            };
        }

        public string GetRoleTextColorClass(string roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "text-red-600",
                "staff" => "text-purple-600",
                "member" => "text-green-600",
                _ => "text-blue-600"
            };
        }

        public string GetRoleBadgeClass(string roleName)
        {
            return roleName?.ToLower() switch
            {
                "admin" => "bg-red-100 text-red-800",
                "staff" => "bg-purple-100 text-purple-800",
                "member" => "bg-green-100 text-green-800",
                _ => "bg-blue-100 text-blue-800"
            };
        }

        public string GetStatusBadgeClass(bool isActive)
        {
            return isActive ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800";
        }

        public string GetPageUrl(int page)
        {
            var queryParams = new List<string>();
            
            // Always add the page parameter
            queryParams.Add($"CurrentPage={page}");
            
            if (!string.IsNullOrEmpty(SearchTerm))
                queryParams.Add($"SearchTerm={Uri.EscapeDataString(SearchTerm)}");
            
            if (!string.IsNullOrEmpty(SelectedRole))
                queryParams.Add($"SelectedRole={Uri.EscapeDataString(SelectedRole)}");
            
            return "?" + string.Join("&", queryParams);
        }
    }

    public class UserStatistics
    {
        public int TotalUsers { get; set; }
        public int MemberCount { get; set; }
        public int StaffCount { get; set; }
        public int AdminsCount { get; set; }
    }
}
