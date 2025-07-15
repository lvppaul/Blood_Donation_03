using BD.RazorPages.Models;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IDonorAvailabilityService _donorAvailabilityService;
        private readonly IBloodRequestService _bloodRequestService;

        public SearchModel(IDonorAvailabilityService donorAvailabilityService, IBloodRequestService bloodRequestService)
        {
            _donorAvailabilityService = donorAvailabilityService;
            _bloodRequestService = bloodRequestService;
        }

        [BindProperty]
        public BloodDonorSearchRequest SearchRequest { get; set; } = new BloodDonorSearchRequest();

        public IEnumerable<DonorAvailabilityResponse> AvailableDonors { get; set; } = new List<DonorAvailabilityResponse>();

        public bool HasSearched { get; set; } = false;

        public async Task OnGetAsync()
        {
            // Load all available donors initially
            AvailableDonors = await _donorAvailabilityService.GetAllAvailableDonorsAsync();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            HasSearched = true;

            if (string.IsNullOrEmpty(SearchRequest.BloodType))
            {
                AvailableDonors = await _donorAvailabilityService.GetAllAvailableDonorsAsync();
                return Page();
            }

            if (SearchRequest.SearchType == "exact")
            {
                AvailableDonors = await _donorAvailabilityService.GetAvailableDonorsByBloodTypeAsync(SearchRequest.BloodType);
            }
            else
            {
                AvailableDonors = await _donorAvailabilityService.SearchCompatibleDonorsAsync(SearchRequest.BloodType);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmDonorAsync(int donorId, string urgencyLevel, string componentType, int amountRequired, string notes)
        {
            try
            {
                // Get donor information
                var donor = await _donorAvailabilityService.GetByIdAsync(donorId);
                if (donor == null)
                {
                    TempData["ErrorMessage"] = "Donor not found. Please try again.";
                    return RedirectToPage();
                }

                // Get current user ID from session (you'll need to implement session management)
                var success = Int32.TryParse(HttpContext.Session.GetString("UserId"), out var currentUserId);
                if (success == false)
                {
                    // For demo purposes, use a default user ID or prompt login
                    TempData["ErrorMessage"] = "Please login to contact donors. Using demo user for now.";
                    //currentUserId = 1; // Default user ID for demo
                }

                // Create blood request
                var bloodRequest = new BloodRequestRequest
                {
                    UserId = currentUserId,
                    BloodType = donor.User.BloodType,
                    ComponentType = componentType,
                    Amount = amountRequired,
                    UrgencyLevel = urgencyLevel,
                    RequestDate = DateTime.Now,
                    StatusRequestId = 1, // Pending status
                    FulfilledDate = null
                };

                // Create the blood request
                var createdRequest = await _bloodRequestService.AddAsync(bloodRequest);

                if (createdRequest != null)
                {
                    TempData["SuccessMessage"] = $"Blood request created successfully! " +
                        $"Donor {donor.User.Name} ({donor.User.BloodType}) has been contacted. " +
                        $"Request ID: {createdRequest.RequestId} | " +
                        $"Component: {createdRequest.ComponentType} | " +
                        $"Amount: {createdRequest.Amount}ml | " +
                        $"Urgency: {createdRequest.UrgencyLevel}";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create blood request. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
