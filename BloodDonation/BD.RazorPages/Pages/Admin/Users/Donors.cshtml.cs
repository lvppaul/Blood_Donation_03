using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin.Users
{
    public class DonorsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IDonorAvailabilityService _donorAvailabilityService;
        private readonly IStatusBloodDonorService _statusBloodDonorService;

        public DonorsModel(IUserService userService, IDonationHistoryService donationHistoryService, IDonorAvailabilityService donorAvailabilityService, IStatusBloodDonorService statusBloodDonorService)
        {
            _userService = userService;
            _donationHistoryService = donationHistoryService;
            _donorAvailabilityService = donorAvailabilityService;
            _statusBloodDonorService = statusBloodDonorService;
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
                var donorUserIds = donorAvailabilities
                    .Where(d => d.User != null)
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

            // Lấy donation histories từ donors có trong DonorAvailabilities
            try
            {
                // Lấy danh sách donor user IDs từ DonorAvailabilities
                var donorAvailabilities = await _donorAvailabilityService.GetAllAsync();
                var donorUserIds = donorAvailabilities
                    .Where(d => d.User != null)
                    .Select(d => d.User.UserId)
                    .Distinct()
                    .ToHashSet();

                // Lấy tất cả donation histories
                var allDonations = await _donationHistoryService.GetAllAsync();
                
                // Lọc chỉ lấy donations từ donors
                var donorDonations = allDonations.Where(d => d.User != null && donorUserIds.Contains(d.User.UserId));
                
                DonationHistories = donorDonations.ToList();

                // Tính toán thống kê
                PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting);
                ConfirmedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Confirmed);
                CompletedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Donated);
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
                }
                catch (Exception)
                {
                    // Final fallback nếu tất cả đều lỗi
                    DonationHistories = new List<DonationHistoryResponse>();
                    PendingDonations = 0;
                    ConfirmedDonations = 0;
                    CompletedDonations = 0;
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
                var success = await _donationHistoryService.UpdateStatusAsync(donationId, newStatus);
                if (success)
                {
                    TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy donation để cập nhật.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật trạng thái.";
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
            // Find corresponding donor availability for this user
            var donorAvailability = _donorAvailabilityService.GetAllAsync().Result
                .FirstOrDefault(d => d.User?.UserId == donor.UserId);

            if (donorAvailability?.StatusDonor == null)
                return "bg-gray-100 text-gray-800"; // Default if no status

            // Map status names to Tailwind CSS classes
            var statusName = donorAvailability.StatusDonor.StatusName?.ToLower();
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
            var donorAvailability = _donorAvailabilityService.GetAllAsync().Result
                .FirstOrDefault(d => d.User?.UserId == donor.UserId);

            if (donorAvailability?.StatusDonor == null)
                return "Not Set";

            // Return the actual status name from database
            return donorAvailability.StatusDonor.StatusName ?? "Not Set";
        }

        public int GetDonorStatus(UserResponse donor)
        {
            var donorAvailability = _donorAvailabilityService.GetAllAsync().Result
                .FirstOrDefault(d => d.User?.UserId == donor.UserId);

            return donorAvailability?.StatusDonor?.StatusDonorId ?? 1; // Default to Available
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
    }
}
