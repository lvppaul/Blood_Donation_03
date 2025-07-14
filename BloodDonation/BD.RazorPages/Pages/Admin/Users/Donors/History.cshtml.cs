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

        public HistoryModel(IUserService userService, IDonationHistoryService donationHistoryService)
        {
            _userService = userService;
            _donationHistoryService = donationHistoryService;
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
                DonationHistories = allDonations.OrderByDescending(d => d.DonationDate ?? DateTime.MinValue).ToList();

                // Tính statistics
                TotalDonations = DonationHistories.Count;
                CompletedDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Donated);
                PendingDonations = DonationHistories.Count(d => GetDonationStatus(d) == DonationStatus.Waiting);
                
                var lastDonation = DonationHistories.FirstOrDefault(d => d.DonationDate.HasValue);
                LastDonationDate = lastDonation?.DonationDate;

                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải lịch sử hiến máu.";
                return RedirectToPage("/Admin/Users/Donors");
            }
        }

        // Cập nhật status từ trang history
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
    }
}
