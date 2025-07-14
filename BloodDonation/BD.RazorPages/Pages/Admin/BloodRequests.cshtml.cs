using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.DTOs.Requests;

namespace BD.RazorPages.Pages.Admin
{
    public class BloodRequestsModel : PageModel
    {
        private readonly IBloodRequestService _bloodRequestService;

        public BloodRequestsModel(IBloodRequestService bloodRequestService)
        {
            _bloodRequestService = bloodRequestService;
        }

        public IEnumerable<BloodRequestResponse> BloodRequests { get; set; } = new List<BloodRequestResponse>();
        public IEnumerable<BloodRequestResponse> PendingRequests { get; set; } = new List<BloodRequestResponse>();
        public IEnumerable<BloodRequestResponse> ApprovedRequests { get; set; } = new List<BloodRequestResponse>();
        public IEnumerable<BloodRequestResponse> CancelledRequests { get; set; } = new List<BloodRequestResponse>();
        public IEnumerable<BloodRequestResponse> FulfilledRequests { get; set; } = new List<BloodRequestResponse>();
        public IEnumerable<BloodRequestResponse> RejectedRequests { get; set; } = new List<BloodRequestResponse>();

        public async Task OnGetAsync()
        {
            try
            {
                var allRequests = await _bloodRequestService.GetAllAsync();
                
                // Sort by urgency: Emergency first, then by date
                var sortedRequests = allRequests
                    .OrderByDescending(r => r.UrgencyLevel?.ToLower() == "emergency")
                    .ThenBy(r => r.RequestDate)
                    .ToList();

                BloodRequests = sortedRequests;
                PendingRequests = sortedRequests.Where(r => r.StatusBloodRequestResponse.StatusRequestId == 1);
                ApprovedRequests = sortedRequests.Where(r => r.StatusBloodRequestResponse.StatusRequestId == 2);
                CancelledRequests = sortedRequests.Where(r => r.StatusBloodRequestResponse.StatusRequestId == 3);
                FulfilledRequests = sortedRequests.Where(r => r.StatusBloodRequestResponse.StatusRequestId == 4);
                RejectedRequests = sortedRequests.Where(r => r.StatusBloodRequestResponse.StatusRequestId == 5);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading blood requests: {ex.Message}";
                BloodRequests = new List<BloodRequestResponse>();
                PendingRequests = new List<BloodRequestResponse>();
                ApprovedRequests = new List<BloodRequestResponse>();
                CancelledRequests = new List<BloodRequestResponse>();
                FulfilledRequests = new List<BloodRequestResponse>();
                RejectedRequests = new List<BloodRequestResponse>();
            }
        }

        public async Task<IActionResult> OnPostApproveRequestAsync(int requestId)
        {
            try
            {
                var request = await _bloodRequestService.GetByIdAsync(requestId);
                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found.";
                    return RedirectToPage();
                }

                // Create update request
                var updateRequest = new BloodRequestRequest
                {
                    UserId = request.User.UserId,
                    BloodType = request.BloodType,
                    ComponentType = request.ComponentType,
                    Amount = request.Amount,
                    UrgencyLevel = request.UrgencyLevel,
                    RequestDate = request.RequestDate,
                    StatusRequestId = 2, // Approved
                    FulfilledDate = request.FulfilledDate
                };

                await _bloodRequestService.UpdateAsync(requestId, updateRequest);

                TempData["SuccessMessage"] = $"Request #{requestId} has been approved successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error approving request: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectRequestAsync(int requestId)
        {
            try
            {
                var request = await _bloodRequestService.GetByIdAsync(requestId);
                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found.";
                    return RedirectToPage();
                }

                // Create update request
                var updateRequest = new BloodRequestRequest
                {
                    UserId = request.User.UserId,
                    BloodType = request.BloodType,
                    ComponentType = request.ComponentType,
                    Amount = request.Amount,
                    UrgencyLevel = request.UrgencyLevel,
                    RequestDate = request.RequestDate,
                    StatusRequestId = 5, // Rejected
                    FulfilledDate = request.FulfilledDate
                };

                await _bloodRequestService.UpdateAsync(requestId, updateRequest);

                TempData["SuccessMessage"] = $"Request #{requestId} has been rejected.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error rejecting request: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFulfillRequestAsync(int requestId)
        {
            try
            {
                var request = await _bloodRequestService.GetByIdAsync(requestId);
                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found.";
                    return RedirectToPage();
                }

                // Create update request
                var updateRequest = new BloodRequestRequest
                {
                    UserId = request.User.UserId,
                    BloodType = request.BloodType,
                    ComponentType = request.ComponentType,
                    Amount = request.Amount,
                    UrgencyLevel = request.UrgencyLevel,
                    RequestDate = request.RequestDate,
                    StatusRequestId = 4, // Fulfilled
                    FulfilledDate = DateTime.Now
                };

                await _bloodRequestService.UpdateAsync(requestId, updateRequest);

                TempData["SuccessMessage"] = $"Request #{requestId} has been marked as fulfilled.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error fulfilling request: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelRequestAsync(int requestId)
        {
            try
            {
                var request = await _bloodRequestService.GetByIdAsync(requestId);
                if (request == null)
                {
                    TempData["ErrorMessage"] = "Request not found.";
                    return RedirectToPage();
                }

                // Create update request
                var updateRequest = new BloodRequestRequest
                {
                    UserId = request.User.UserId,
                    BloodType = request.BloodType,
                    ComponentType = request.ComponentType,
                    Amount = request.Amount,
                    UrgencyLevel = request.UrgencyLevel,
                    RequestDate = request.RequestDate,
                    StatusRequestId = 3, // Cancelled
                    FulfilledDate = request.FulfilledDate
                };

                await _bloodRequestService.UpdateAsync(requestId, updateRequest);

                TempData["SuccessMessage"] = $"Request #{requestId} has been cancelled.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error cancelling request: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
