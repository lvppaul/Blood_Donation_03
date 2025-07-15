using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Users
{
    // Request model for updating donor status
    public class UpdateDonorStatusRequest
    {
        public int UserId { get; set; }
        public int Status { get; set; }
    }

    public class DonorsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IDonorAvailabilityService _donorAvailabilityService;
        private readonly IStatusBloodDonorService _statusBloodDonorService;
        private readonly IBloodInventoryService _bloodInventoryService;

        public DonorsModel(IUserService userService, IDonationHistoryService donationHistoryService, IDonorAvailabilityService donorAvailabilityService, IStatusBloodDonorService statusBloodDonorService, IBloodInventoryService bloodInventoryService)
        {
            _userService = userService;
            _donationHistoryService = donationHistoryService;
            _donorAvailabilityService = donorAvailabilityService;
            _statusBloodDonorService = statusBloodDonorService;
            _bloodInventoryService = bloodInventoryService;
        }

        // Danh sách donor để hiển thị
        public List<UserResponse> Donors { get; set; } = new();

        // Danh sách donation history
        public List<DonationHistoryResponse> DonationHistories { get; set; } = new();

        // Danh sách trạng thái donor
        public List<StatusBloodDonorResponse> StatusBloodDonors { get; set; } = new();

        // Thống kê
        public int TotalDonors { get; set; }
        public int PendingDonations { get; set; }
        public int ConfirmedDonations { get; set; }
        public int CompletedDonations { get; set; }
        public int RejectedDonations { get; set; }
        public int CanceledDonations { get; set; }

        // Tìm kiếm
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        // Tab hiện tại
        [BindProperty(SupportsGet = true)]
        public string ActiveTab { get; set; } = "donors";

        public async Task OnGetAsync()
        {
            // Lấy danh sách trạng thái donor từ database
            try
            {
                var statuses = await _statusBloodDonorService.GetAllAsync();
                StatusBloodDonors = statuses.ToList();
            }
            catch (Exception)
            {
                StatusBloodDonors = new List<StatusBloodDonorResponse>();
            }

            // Lấy donors từ DonorAvailability - chỉ những user có đăng ký làm donor
            try
            {
                var donorAvailabilities = await _donorAvailabilityService.GetAllAsync();

                // Lấy donor availability gần nhất theo available date của từng user
                var latestDonorAvailabilities = donorAvailabilities
                    .Where(d => d.User != null)
                    .GroupBy(d => d.User.UserId)
                    .Select(g => g.OrderByDescending(d => d.AvailableDate).First())
                    .ToList();

                var donorUserIds = latestDonorAvailabilities
                    .Select(d => d.User.UserId)
                    .Distinct()
                    .ToList();

                var allUsers = await _userService.GetAllAsync();
                var donors = allUsers.Where(u => donorUserIds.Contains(u.UserId));

                // Lọc theo search term nếu có
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    donors = donors.Where(u =>
                        (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    );
                }

                Donors = donors.ToList();
                TotalDonors = Donors.Count;
            }
            catch (Exception)
            {
                // Fallback - lấy tất cả users có role Member nếu DonorAvailability service lỗi
                var allUsers = await _userService.GetAllAsync();
                var donors = allUsers.Where(u => u.Role != null && u.Role.Name.Equals("Member", StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    donors = donors.Where(u =>
                        (!string.IsNullOrEmpty(u.Name) && u.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    );
                }

                Donors = donors.ToList();
                TotalDonors = Donors.Count;
            }

            // Lấy tất cả donation histories (không cần lọc theo latest availability)
            try
            {
                // Lấy tất cả donation histories từ users có role Member
                var allDonations = await _donationHistoryService.GetAllAsync();
                var memberDonations = allDonations.Where(d => d.User?.Role?.Name?.Equals("Member", StringComparison.OrdinalIgnoreCase) == true);

                DonationHistories = memberDonations.ToList();

                // Tính toán thống kê
                PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting);
                ConfirmedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Confirmed);
                CompletedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Donated);
                RejectedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Rejected);
                CanceledDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Canceled);
            }
            catch (Exception)
            {
                // Fallback nếu service lỗi
                try
                {
                    // Fallback - lấy tất cả donations từ users có role Member
                    var allDonations = await _donationHistoryService.GetAllAsync();
                    var memberDonations = allDonations.Where(d => d.User?.Role?.Name?.Equals("Member", StringComparison.OrdinalIgnoreCase) == true);

                    DonationHistories = memberDonations.ToList();

                    PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting);
                    ConfirmedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Confirmed);
                    CompletedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Donated);
                    RejectedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Rejected);
                    CanceledDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Canceled);
                }
                catch (Exception)
                {
                    // Final fallback nếu tất cả đều lỗi
                    DonationHistories = new List<DonationHistoryResponse>();
                    PendingDonations = 0;
                    ConfirmedDonations = 0;
                    CompletedDonations = 0;
                    RejectedDonations = 0;
                    CanceledDonations = 0;
                }
            }
        }

        // Helper method để lấy status từ DonationHistoryResponse
        public DonationStatus GetDonationStatus(DonationHistoryResponse donation)
        {
            // Sử dụng Status field từ response thay vì logic mock
            return donation.Status;
        }

        // Cập nhật status
        public async Task<IActionResult> OnPostUpdateStatusAsync(int donationId, DonationStatus newStatus)
        {
            try
            {
                // Get current donation details before update
                var currentDonation = await _donationHistoryService.GetByIdAsync(donationId);
                if (currentDonation == null)
                {
                    TempData["ErrorMessage"] = "Donation not found.";
                    return RedirectToPage(new { ActiveTab = "donations" });
                }

                // Check if current status is already Donated, Rejected, or Canceled - prevent further updates
                if (currentDonation.Status == DonationStatus.Donated ||
                    currentDonation.Status == DonationStatus.Rejected ||
                    currentDonation.Status == DonationStatus.Canceled)
                {
                    TempData["ErrorMessage"] = "Can't update status because donation has been completed.";
                    return RedirectToPage(new { ActiveTab = "donations" });
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
                    TempData["ErrorMessage"] = "Donation not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error when update status:: {ex.Message}";
            }

            return RedirectToPage(new { ActiveTab = "donations" });
        }

        // Hàm lấy initials cho avatar
        public string GetInitials(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
            return string.Concat(parts[0][0], parts[^1][0]).ToUpper();
        }

        // Donor status methods
        public string GetDonorStatusClass(UserResponse donor)
        {
            // Find latest donor availability for this user
            var donorAvailabilities = _donorAvailabilityService.GetAllAsync().Result;
            var latestDonorAvailability = donorAvailabilities
                .Where(d => d.User?.UserId == donor.UserId)
                .OrderByDescending(d => d.AvailableDate)
                .FirstOrDefault();

            if (latestDonorAvailability?.StatusDonor == null)
                return "bg-gray-100 text-gray-800"; // Default if no status

            // Map status names to Tailwind CSS classes
            var statusName = latestDonorAvailability.StatusDonor.StatusName?.ToLower();
            return statusName switch
            {
                "available" => "bg-green-100 text-green-800",
                "unavailable" => "bg-yellow-100 text-yellow-800",
                "recovery period" => "bg-blue-100 text-blue-800",
                "medical hold" => "bg-red-100 text-red-800",
                _ => "bg-gray-100 text-gray-800" // Unknown
            };
        }

        public string GetDonorStatusText(UserResponse donor)
        {
            var donorAvailabilities = _donorAvailabilityService.GetAllAsync().Result;
            var latestDonorAvailability = donorAvailabilities
                .Where(d => d.User?.UserId == donor.UserId)
                .OrderByDescending(d => d.AvailableDate)
                .FirstOrDefault();

            if (latestDonorAvailability?.StatusDonor == null)
                return "Not Set";

            // Return the actual status name from database
            return latestDonorAvailability.StatusDonor.StatusName ?? "Not Set";
        }

        public int GetDonorStatus(UserResponse donor)
        {
            var donorAvailabilities = _donorAvailabilityService.GetAllAsync().Result;
            var latestDonorAvailability = donorAvailabilities
                .Where(d => d.User?.UserId == donor.UserId)
                .OrderByDescending(d => d.AvailableDate)
                .FirstOrDefault();

            return latestDonorAvailability?.StatusDonor?.StatusDonorId ?? 1; // Default to Available
        }

        // Get all status as JSON for JavaScript
        public string GetStatusListJson()
        {
            var statusList = StatusBloodDonors.Select(s => new { id = s.StatusDonorId, name = s.StatusName }).ToList();
            return System.Text.Json.JsonSerializer.Serialize(statusList);
        }

        // Handler for updating donor status
        public async Task<IActionResult> OnPostUpdateDonorStatusAsync([FromBody] UpdateDonorStatusRequest request)
        {
            try
            {
                // Load status list if not already loaded
                if (!StatusBloodDonors.Any())
                {
                    var statuses = await _statusBloodDonorService.GetAllAsync();
                    StatusBloodDonors = statuses.ToList();
                }

                // Find donor availability for this user
                var donorAvailabilities = await _donorAvailabilityService.GetAllAsync();
                var donorAvailability = donorAvailabilities.FirstOrDefault(d => d.User?.UserId == request.UserId);

                if (donorAvailability == null)
                {
                    return new JsonResult(new { success = false, message = "Donor not found" });
                }

                // Validate status exists
                var statusExists = StatusBloodDonors.Any(s => s.StatusDonorId == request.Status);
                if (!statusExists)
                {
                    return new JsonResult(new { success = false, message = "Invalid status selected" });
                }

                // Update the donor availability with new status
                var updateRequest = new DonorAvailabilityRequest
                {
                    UserId = donorAvailability.User.UserId,
                    StatusDonorId = request.Status,
                    LastDonationDate = donorAvailability.LastDonationDate,
                    RecoveryReminderDate = donorAvailability.RecoveryReminderDate,
                    AvailableDate = donorAvailability.AvailableDate
                };

                var success = await _donorAvailabilityService.UpdateAsync(donorAvailability.AvailabilityId, updateRequest);

                if (success != null)
                {
                    return new JsonResult(new { success = true, message = "Status updated successfully" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Failed to update status" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Cập nhật blood inventory khi donation status thành Donated
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
                throw new Exception($"Fail updating Blood Inventory: {ex.Message}");
            }
        }

        // Cập nhật trạng thái donor khi donation hoàn thành
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
                            //RecoveryReminderDate = DateOnly.FromDateTime(DateTime.Today.AddDays(56)) // 8 weeks recovery period
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
