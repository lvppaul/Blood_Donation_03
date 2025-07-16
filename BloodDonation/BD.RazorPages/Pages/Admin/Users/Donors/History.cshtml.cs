using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Users.Donors
{
    public class HistoryModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IDonorAvailabilityService _donorAvailabilityService;

        public HistoryModel(IUserService userService, IDonationHistoryService donationHistoryService, IBloodInventoryService bloodInventoryService, IDonorAvailabilityService donorAvailabilityService)
        {
            _userService = userService;
            _donationHistoryService = donationHistoryService;
            _bloodInventoryService = bloodInventoryService;
            _donorAvailabilityService = donorAvailabilityService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        // Donor info
        public string DonorName { get; set; } = string.Empty;
        public string DonorEmail { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public DateTime? JoinedDate { get; set; }

        // Donation histories
        public List<DonationHistoryResponse> DonationHistories { get; set; } = new();

        // Statistics
        public int TotalDonations { get; set; }
        public int CompletedDonations { get; set; }
        public int PendingDonations { get; set; }
        public int RejectedDonations { get; set; }
        public int CanceledDonations { get; set; }
        public DateTime? LastDonationDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy thông tin donor
                var donor = await _userService.GetByIdAsync(Id);
                if (donor == null)
                {
                    return NotFound();
                }

                DonorName = donor.Name ?? "Unknown";
                DonorEmail = donor.Email ?? "Unknown";
                BloodType = donor.BloodType ?? "Unknown";
                JoinedDate = donor.CreatedAt;

                // Lấy donation history cho donor này
                var allDonations = await _donationHistoryService.GetByUserIdAsync(Id);
                DonationHistories = allDonations.OrderByDescending(d => d.CreatedDate).ToList();

                // Tính statistics
                TotalDonations = DonationHistories.Count;
                CompletedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Donated);
                PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting);
                var confirmedCount = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Confirmed);
                RejectedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Rejected);
                CanceledDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Canceled);

                // For PendingDonations, include both Waiting and Confirmed
                PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting || GetDonationStatus(d) == DonationStatus.Confirmed);

                var lastCompletedDonation = DonationHistories.FirstOrDefault(d => GetDonationStatus(d) == DonationStatus.Donated && d.DonationDate.HasValue);
                LastDonationDate = lastCompletedDonation?.DonationDate;

                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error when loading donation History.";
                return RedirectToPage("/Admin/Users/Donors");
            }
        }

        // Cập nhật status từ trang history
        public async Task<IActionResult> OnPostUpdateStatusAsync(int donationId, DonationStatus newStatus)
        {
            try
            {
                // Get current donation details before update
                var currentDonation = await _donationHistoryService.GetByIdAsync(donationId);
                if (currentDonation == null)
                {
                    TempData["ErrorMessage"] = "Donation not found.";
                    return RedirectToPage(new { Id = Id });
                }

                // Check if current status is already Donated, Rejected, or Canceled - prevent further updates
                if (currentDonation.Status == DonationStatus.Donated ||
                    currentDonation.Status == DonationStatus.Rejected ||
                    currentDonation.Status == DonationStatus.Canceled)
                {
                    TempData["ErrorMessage"] = "Can't update status because donation has been completed.";
                    return RedirectToPage(new { Id = Id });
                }

                // Update donation status
                var success = await _donationHistoryService.UpdateStatusAsync(donationId, newStatus);
                if (success)
                {
                    // If status changed to Donated, update blood inventory and donor availability
                    if (newStatus == DonationStatus.Donated)
                    {
                        try
                        {
                            await UpdateBloodInventoryAsync(currentDonation);
                            await UpdateDonorAvailabilityStatusAsync(currentDonation.User.UserId, 3); // Set status_donor_id to 3
                            TempData["SuccessMessage"] = "Update Successfully! Blood Inventory and donor status has been updated.";
                        }
                        catch (Exception inventoryEx)
                        {
                            TempData["SuccessMessage"] = "Update status Successfully! But error update Blood Inventory or Status Donor.";
                            TempData["ErrorMessage"] = $"Update Error: {inventoryEx.Message}";
                        }
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Update status Successfully!";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Donation not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error when update status: {ex.Message}";
            }

            return RedirectToPage(new { Id = Id });
        }

        // Helper method để lấy status từ DonationHistoryResponse
        public DonationStatus GetDonationStatus(DonationHistoryResponse donation)
        {
            // Sử dụng Status field từ response thay vì logic mock
            return donation.Status;
        }

        // Hàm lấy initials cho avatar
        public string GetInitials(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return string.Concat(parts[0][0], parts[^1][0]).ToUpper();
        }

        // Cập nhật blood inventory khi donation status thành Donated
        private async Task UpdateBloodInventoryAsync(BD.Repositories.Models.DTOs.Responses.DonationHistoryResponse donation)
        {
            try
            {
                // Validate donation data
                if (donation.Facility == null || donation.Facility.FacilityId <= 0)
                {
                    throw new Exception("Facility information is missing or invalid");
                }

                if (string.IsNullOrEmpty(donation.BloodType))
                {
                    throw new Exception("Blood type is missing");
                }

                if (string.IsNullOrEmpty(donation.ComponentType))
                {
                    throw new Exception("Component type is missing");
                }

                if (donation.Amount <= 0)
                {
                    throw new Exception("Invalid donation amount");
                }

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

        // Update donor availability status when donation is completed
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
                                                           //  LastDonationDate = DateOnly.FromDateTime(DateTime.Today), // Update last donation date
                                                           //  RecoveryReminderDate = DateOnly.FromDateTime(DateTime.Today.AddDays(56)) // 8 weeks recovery period
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
