using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Profile
{
    public class BloodRequestsModel : PageModel
    {
        private readonly IBloodRequestService _bloodRequestService;
        private readonly IUserService _userService;

        public BloodRequestsModel(IBloodRequestService bloodRequestService, IUserService userService)
        {
            _bloodRequestService = bloodRequestService;
            _userService = userService;
        }

        public string UserName { get; set; } = "";
        public IEnumerable<BloodRequestResponse> BloodRequests { get; set; } = new List<BloodRequestResponse>();
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int FulfilledRequests { get; set; }
        public int EmergencyRequests { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get current user ID from session or claims
                var userIdClaim = HttpContext.Session.GetString("UserId"); ;

                if (userIdClaim == null)
                {
                    // If no user claims, redirect to login
                    return RedirectToPage("/Login");
                }

                int currentUserId = 0;
                currentUserId = int.Parse(userIdClaim);
                // Get user by email if userId claim is not available
                var user = await _userService.GetByIdAsync(currentUserId);
                if (user != null)
                {
                    currentUserId = user.UserId;
                    UserName = user.Name;
                }


                if (currentUserId == 0)
                {
                    return RedirectToPage("/Login");
                }

                // Get all blood requests for the current user
                var allRequests = await _bloodRequestService.GetAllAsync();
                var userRequests = allRequests.Where(r => r.User?.UserId == currentUserId).ToList();

                // Sort by date (most recent first) and emergency status
                BloodRequests = userRequests
                    .OrderByDescending(r => r.UrgencyLevel?.ToLower() == "emergency")
                    .ThenByDescending(r => r.RequestDate)
                    .ToList();

                // Get user name if not already set
                if (string.IsNullOrEmpty(UserName) && BloodRequests.Any())
                {
                    UserName = BloodRequests.First().User?.Name ?? "Unknown User";
                }

                // Calculate statistics
                TotalRequests = BloodRequests.Count();
                PendingRequests = BloodRequests.Count(r => r.StatusBloodRequestResponse.StatusRequestId == 1);
                FulfilledRequests = BloodRequests.Count(r => r.StatusBloodRequestResponse.StatusRequestId == 4);
                EmergencyRequests = BloodRequests.Count(r => r.UrgencyLevel?.ToLower() == "emergency");

                return Page();
            }
            catch (Exception ex)
            {
                // Log error and redirect to profile with error message
                TempData["ErrorMessage"] = "Error loading blood requests. Please try again.";
                return RedirectToPage("/Profile");
            }
        }
    }
}
