using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using BD.Services.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class BloodUnitsModel : PageModel
    {
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IMedicalFacilityService _medicalFacilityService;
        private readonly IBloodRequestService _bloodRequestService;

        public IEnumerable<BloodInventoryResponse> BloodUnits { get; set; } = Enumerable.Empty<BloodInventoryResponse>();
        public IEnumerable<BloodTypeOverview> BloodTypeOverviews { get; set; } = Enumerable.Empty<BloodTypeOverview>();
        public IEnumerable<MedicalFacilityResponse> MedicalFacilities { get; set; } = Enumerable.Empty<MedicalFacilityResponse>();
        public IEnumerable<BloodRequestResponse> AvailableRequests { get; set; } = Enumerable.Empty<BloodRequestResponse>();

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;
        public int TotalBloodUnits { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalBloodUnits / PageSize);

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string SelectedBloodType { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public int? SelectedFacilityId { get; set; }

        public BloodUnitsModel(IBloodInventoryService bloodInventoryService, IMedicalFacilityService medicalFacilityService, IBloodRequestService bloodRequestService)
        {
            _bloodInventoryService = bloodInventoryService;
            _medicalFacilityService = medicalFacilityService;
            _bloodRequestService = bloodRequestService;
        }

        public async Task OnGetAsync()
        {
            if (CurrentPage < 1) CurrentPage = 1;
            if (!new[] { 10, 25, 50 }.Contains(PageSize)) PageSize = 10;

            try
            {
                (var units, TotalBloodUnits) = await _bloodInventoryService.GetFilteredAsync(
                    SearchTerm, SelectedBloodType, SelectedFacilityId, CurrentPage, PageSize);
                BloodUnits = units ?? Enumerable.Empty<BloodInventoryResponse>();

                BloodTypeOverviews = await _bloodInventoryService.GetBloodTypeOverviewAsync() ?? Enumerable.Empty<BloodTypeOverview>();
                MedicalFacilities = await _medicalFacilityService.GetAllAsync() ?? Enumerable.Empty<MedicalFacilityResponse>();

                // Load available blood requests (non-fulfilled requests)
                var allRequests = await _bloodRequestService.GetAllAsync();
                AvailableRequests = allRequests.Where(r => r.StatusBloodRequestResponse?.StatusName != "Fulfilled" &&
                                                          r.StatusBloodRequestResponse?.StatusName != "Canceled" &&
                                                          r.StatusBloodRequestResponse?.StatusName != "Rejected" &&
                                                          r.IsDeleted != true).ToList();

                if (CurrentPage > TotalPages && TotalPages > 0)
                {
                    CurrentPage = TotalPages;
                    (BloodUnits, TotalBloodUnits) = await _bloodInventoryService.GetFilteredAsync(
                        SearchTerm, SelectedBloodType, SelectedFacilityId, CurrentPage, PageSize);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading blood units. Please try again later.";
                Console.WriteLine($"Error in BloodUnits OnGetAsync: {ex.Message}");
                BloodUnits = Enumerable.Empty<BloodInventoryResponse>();
                BloodTypeOverviews = Enumerable.Empty<BloodTypeOverview>();
                MedicalFacilities = Enumerable.Empty<MedicalFacilityResponse>();
                AvailableRequests = Enumerable.Empty<BloodRequestResponse>();
                TotalBloodUnits = 0;
            }
        }

        public async Task<IActionResult> OnPostDisposeAsync(int id)
        {
            try
            {
                // Get the blood unit to dispose
                var bloodUnit = await _bloodInventoryService.GetByIdAsync(id);
                if (bloodUnit == null)
                {
                    TempData["ErrorMessage"] = "Blood unit not found.";
                    return RedirectToPage();
                }

                // Perform soft delete (dispose)
                await _bloodInventoryService.DeleteAsync(id);

                TempData["SuccessMessage"] = $"Blood unit #{id} has been successfully disposed. Blood Type: {bloodUnit.BloodType}, Component: {bloodUnit.ComponentType}, Amount: {bloodUnit.Amount} units.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error disposing blood unit: {ex.Message}";
            }

            // Redirect back to the same page with current filters and pagination
            return RedirectToPage(new
            {
                SearchTerm = SearchTerm,
                SelectedBloodType = SelectedBloodType,
                SelectedFacilityId = SelectedFacilityId,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            });
        }

        public async Task<IActionResult> OnPostTransferAsync(int inventoryId, int requestId)
        {
            try
            {
                // Get the blood unit to transfer
                var bloodUnit = await _bloodInventoryService.GetByIdAsync(inventoryId);
                if (bloodUnit == null)
                {
                    TempData["ErrorMessage"] = "Blood unit not found.";
                    return RedirectToPage();
                }

                // Get the blood request
                var bloodRequest = await _bloodRequestService.GetByIdAsync(requestId);
                if (bloodRequest == null)
                {
                    TempData["ErrorMessage"] = "Blood request not found.";
                    return RedirectToPage();
                }

                // Check if blood types match
                if (bloodUnit.BloodType != bloodRequest.BloodType)
                {
                    TempData["ErrorMessage"] = "Blood type mismatch. Cannot transfer blood.";
                    return RedirectToPage();
                }

                // Check if component types match
                if (bloodUnit.ComponentType != bloodRequest.ComponentType)
                {
                    TempData["ErrorMessage"] = "Component type mismatch. Cannot transfer blood.";
                    return RedirectToPage();
                }

                // Check if blood unit has enough amount
                if (bloodUnit.Amount < bloodRequest.Amount)
                {
                    TempData["ErrorMessage"] = $"Insufficient blood amount in inventory. Available: {bloodUnit.Amount} units, Requested: {bloodRequest.Amount} units.";
                    return RedirectToPage();
                }

                // Additional validation: Check if request amount is valid (positive)
                if (bloodRequest.Amount <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid request amount. Amount must be greater than 0.";
                    return RedirectToPage();
                }

                // Check if request is already fulfilled or canceled or rejected
                if (bloodRequest.StatusBloodRequestResponse?.StatusName == "Fulfilled")
                {
                    TempData["ErrorMessage"] = "Blood request is already fulfilled.";
                    return RedirectToPage();
                }

                if (bloodRequest.StatusBloodRequestResponse?.StatusName == "Canceled")
                {
                    TempData["ErrorMessage"] = "Blood request has been canceled.";
                    return RedirectToPage();
                }

                if (bloodRequest.StatusBloodRequestResponse?.StatusName == "Rejected")
                {
                    TempData["ErrorMessage"] = "Blood request has been rejected.";
                    return RedirectToPage();
                }

                // Calculate the updated amount (can be 0)
                var updatedAmount = bloodUnit.Amount - bloodRequest.Amount;

                // Update blood inventory - reduce amount (even if it becomes 0)
                var updateInventoryRequest = new BD.Repositories.Models.DTOs.Requests.BloodInventoryRequest
                {
                    FacilityId = bloodUnit.Facility.FacilityId,
                    ComponentType = bloodUnit.ComponentType,
                    Amount = updatedAmount, // This can be 0
                    ExpiredDate = bloodUnit.ExpiredDate,
                    StatusInventoryId = bloodUnit.StatusBloodInventory.StatusInventoryId,
                    BloodType = bloodUnit.BloodType
                };

                var updateInventoryResult = await _bloodInventoryService.UpdateAsync(bloodUnit.InventoryId, updateInventoryRequest);
                if (updateInventoryResult == null)
                {
                    TempData["ErrorMessage"] = "Failed to update blood inventory.";
                    return RedirectToPage();
                }

                // Update blood request status to fulfilled
                var updateRequestRequest = new BD.Repositories.Models.DTOs.Requests.BloodRequestRequest
                {
                    UserId = bloodRequest.User.UserId,
                    BloodType = bloodRequest.BloodType,
                    ComponentType = bloodRequest.ComponentType,
                    Amount = bloodRequest.Amount,
                    UrgencyLevel = bloodRequest.UrgencyLevel,
                    RequestDate = bloodRequest.RequestDate,
                    StatusRequestId = 4,
                    FulfilledDate = DateTime.Now
                };

                var updateRequestResult = await _bloodRequestService.UpdateAsync(bloodRequest.RequestId, updateRequestRequest);
                if (updateRequestResult == null)
                {
                    TempData["ErrorMessage"] = "Failed to update blood request status.";
                    return RedirectToPage();
                }

                TempData["SuccessMessage"] = $"Blood successfully transferred! Unit #{inventoryId} transferred to Request #{requestId}. " +
                                           $"Blood Type: {bloodUnit.BloodType}, Component: {bloodUnit.ComponentType}, Amount: {bloodRequest.Amount} units." +
                                           (updatedAmount == 0 ? " The blood unit amount is now 0." : $" Remaining amount: {updatedAmount} units.");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error transferring blood: {ex.Message}";
            }

            // Redirect back to the same page with current filters and pagination
            return RedirectToPage(new
            {
                SearchTerm = SearchTerm,
                SelectedBloodType = SelectedBloodType,
                SelectedFacilityId = SelectedFacilityId,
                CurrentPage = CurrentPage,
                PageSize = PageSize
            });
        }
    }
}
