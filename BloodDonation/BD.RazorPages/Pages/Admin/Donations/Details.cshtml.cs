using BD.RazorPages.Models;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Donations
{
    public class DetailsModel : PageModel
    {
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IUserService _userService;
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IDonorAvailabilityService _donorAvailabilityService;

        public DetailsModel(IDonationHistoryService donationHistoryService, IUserService userService, IBloodInventoryService bloodInventoryService, IDonorAvailabilityService donorAvailabilityService)
        {
            _donationHistoryService = donationHistoryService;
            _userService = userService;
            _bloodInventoryService = bloodInventoryService;
            _donorAvailabilityService = donorAvailabilityService;
        }

        public DonationHistory? DonationDetails { get; set; }
        public DonationStatusViewModel? StatusViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var donation = await _donationHistoryService.GetByIdAsync(id);
                if (donation == null)
                {
                    TempData["ErrorMessage"] = "Donation not found.";
                    return RedirectToPage("/Admin/Users/Donors", new { ActiveTab = "donations" });
                }

                // Map from DonationHistoryResponse to DonationHistory entity for display
                DonationDetails = new DonationHistory
                {
                    DonationId = donation.DonationId,
                    Amount = donation.Amount,
                    BloodType = donation.BloodType,
                    ComponentType = donation.ComponentType,
                    DonationDate = donation.DonationDate,
                    Status = donation.Status,
                    CreatedDate = donation.CreatedDate,
                    ConfirmedDate = donation.ConfirmedDate,
                    IsDeleted = donation.IsDeleted,
                    // Create User object for display
                    User = new User
                    {
                        UserId = donation.User.UserId,
                        Name = donation.User.Name,
                        Email = donation.User.Email,
                        BloodType = donation.User.BloodType,
                        Address = donation.User.Address,
                        Phone = donation.User.Phone
                    }
                };

                StatusViewModel = new DonationStatusViewModel
                {
                    DonationId = donation.DonationId,
                    DonorName = donation.User.Name,
                    BloodType = donation.BloodType,
                    Status = donation.Status,
                    CreatedDate = donation.CreatedDate,
                    ConfirmedDate = donation.ConfirmedDate,
                    DonationDate = donation.DonationDate
                };

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error when loading donation: {ex.Message}";
                return RedirectToPage("/Admin/Users/Donors", new { ActiveTab = "donations" });
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int donationId, DonationStatus newStatus)
        {
            try
            {
                // Get current donation details before update
                var currentDonation = await _donationHistoryService.GetByIdAsync(donationId);
                if (currentDonation == null)
                {
                    TempData["ErrorMessage"] = "Donation not found.";
                    return RedirectToPage(new { id = donationId });
                }

                // Check if current status is already Donated - prevent further updates
                if (currentDonation.Status == DonationStatus.Donated)
                {
                    TempData["ErrorMessage"] = "Can't update status because donation has been completed.";
                    return RedirectToPage(new { id = donationId });
                }

                // Update donation status
                var success = await _donationHistoryService.UpdateStatusAsync(donationId, newStatus);
                if (success)
                {
                    // If status changed to Donated, update blood inventory and donor availability
                    if (newStatus == DonationStatus.Donated)
                    {
                        await UpdateBloodInventoryAsync(currentDonation);
                        await UpdateDonorAvailabilityStatusAsync(currentDonation.User.UserId, 3); // Set status_donor_id to 3
                    }

                    TempData["SuccessMessage"] = newStatus == DonationStatus.Donated
                        ? "Update Successfully! Blood Inventory and donor status has been updated."
                        : "Update Successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Donation not found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error when update status: {ex.Message}";
            }

            return RedirectToPage(new { id = donationId });
        }

        private async Task UpdateBloodInventoryAsync(BD.Repositories.Models.DTOs.Responses.DonationHistoryResponse donation)
        {
            try
            {
                // Get existing blood inventory for the same facility, blood type, and component type
                var inventories = await _bloodInventoryService.GetFilteredAsync(
                    facilityId: donation.Facility.FacilityId,
                    bloodType: donation.BloodType
                );

                // Find matching inventory by component type
                var matchingInventory = inventories.Item1.FirstOrDefault(i =>
                    i.ComponentType == donation.ComponentType &&
                    i.IsDeleted != true);

                if (matchingInventory != null)
                {
                    // Update existing inventory - increase amount
                    var updateRequest = new BD.Repositories.Models.DTOs.Requests.BloodInventoryRequest
                    {
                        FacilityId = matchingInventory.Facility.FacilityId,
                        ComponentType = matchingInventory.ComponentType,
                        Amount = matchingInventory.Amount + donation.Amount, // Add donated amount
                        ExpiredDate = matchingInventory.ExpiredDate,
                        StatusInventoryId = matchingInventory.StatusBloodInventory.StatusInventoryId,
                        BloodType = matchingInventory.BloodType
                    };

                    await _bloodInventoryService.UpdateAsync(matchingInventory.InventoryId, updateRequest);
                }
                else
                {
                    // Create new inventory entry
                    var createRequest = new BD.Repositories.Models.DTOs.Requests.BloodInventoryRequest
                    {
                        FacilityId = donation.Facility.FacilityId,
                        ComponentType = donation.ComponentType,
                        Amount = donation.Amount,
                        ExpiredDate = DateOnly.FromDateTime(DateTime.Today.AddDays(42)), // Default 42 days expiry
                        StatusInventoryId = 1, // Assuming 1 is "Available" status
                        BloodType = donation.BloodType
                    };

                    await _bloodInventoryService.AddAsync(createRequest);
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the main operation
                throw new Exception($"Fail Updating Blood Inventory: {ex.Message}");
            }
        }

        private async Task UpdateDonorAvailabilityStatusAsync(int userId, int statusDonorId)
        {
            try
            {
                // Get all donor availability records for this user
                var donorAvailabilities = await _donorAvailabilityService.GetAllAsync();
                var userDonorAvailabilities = donorAvailabilities
                    .Where(d => d.User?.UserId == userId)
                    .ToList();

                if (userDonorAvailabilities.Any())
                {
                    // Update the most recent donor availability record
                    var latestDonorAvailability = userDonorAvailabilities
                        .OrderByDescending(d => d.AvailableDate)
                        .FirstOrDefault();

                    if (latestDonorAvailability != null)
                    {
                        // Create update request
                        var updateRequest = new BD.Repositories.Models.DTOs.Requests.DonorAvailabilityRequest
                        {
                            UserId = latestDonorAvailability.User.UserId,
                            AvailableDate = latestDonorAvailability.AvailableDate,
                            StatusDonorId = statusDonorId, // Set to 3 for donated status
                                                           // LastDonationDate = DateOnly.FromDateTime(DateTime.Today), // Update last donation date
                                                           // RecoveryReminderDate = DateOnly.FromDateTime(DateTime.Today.AddDays(56)) // 8 weeks recovery period
                        };

                        await _donorAvailabilityService.UpdateAsync(latestDonorAvailability.AvailabilityId, updateRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the main operation
                throw new Exception($"Fail updating Blood donor: {ex.Message}");
            }
        }
    }
}
