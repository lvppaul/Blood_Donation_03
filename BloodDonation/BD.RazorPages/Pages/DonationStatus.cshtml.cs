using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;

namespace BD.RazorPages.Pages
{
    public class DonationStatusModel : PageModel
    {
        private readonly IDonationHistoryService _donationHistoryService;

        public DonationStatusModel(IDonationHistoryService donationHistoryService)
        {
            _donationHistoryService = donationHistoryService;
        }

        [BindProperty]
        public DonationHistory? DonationHistory { get; set; }

        [BindProperty]
        public int DonationId { get; set; }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                DonationId = id.Value;
                
                try
                {
                    // Try to get real data from service
                    var donationResponse = await _donationHistoryService.GetByIdAsync(id.Value);
                    
                    if (donationResponse != null)
                    {
                        // Map from response to entity for display
                        DonationHistory = new DonationHistory
                        {
                            DonationId = donationResponse.DonationId,
                            UserId = donationResponse.User?.UserId ?? 1, // Use User.UserId if available
                            RequestId = donationResponse.DonationRequest,
                            FacilityId = donationResponse.Facility?.FacilityId ?? 1, // Use Facility.FacilityId if available
                            Amount = donationResponse.Amount,
                            DonationDate = donationResponse.DonationDate,
                            BloodType = donationResponse.BloodType,
                            ComponentType = donationResponse.ComponentType,
                            Status = DonationStatus.Waiting, // Default status, can be enhanced later
                            CreatedDate = donationResponse.DonationDate ?? DateTime.Now,
                            IsDeleted = donationResponse.IsDeleted
                        };
                    }
                    else
                    {
                        // Fallback to mock data for demo
                        CreateMockData(id.Value);
                    }
                }
                catch (Exception)
                {
                    // Fallback to mock data if service fails
                    CreateMockData(id.Value);
                }
            }
            else
            {
                // Default demo data
                CreateMockData(0);
            }
        }

        private void CreateMockData(int id)
        {
            DonationHistory = new DonationHistory
            {
                DonationId = id,
                UserId = 1,
                RequestId = 1,
                FacilityId = 1,
                Amount = 2,
                BloodType = "O+",
                ComponentType = "Whole Blood",
                Status = (DonationStatus)(id % 3), // Demo: cycle through statuses
                CreatedDate = DateTime.Now.AddDays(-2),
                ConfirmedDate = id >= 1 ? DateTime.Now.AddDays(-1) : null,
                DonationDate = id >= 2 ? DateTime.Now : null, // DonationDate represents completion
                IsDeleted = false
            };
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            if (DonationHistory == null) return Page();

            try
            {
                // Use service to update status
                var success = await _donationHistoryService.UpdateStatusAsync(
                    DonationHistory.DonationId, 
                    DonationHistory.Status);

                if (success)
                {
                    TempData["SuccessMessage"] = "Trạng thái hiến máu đã được cập nhật thành công!";
                    return RedirectToPage(new { id = DonationHistory.DonationId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin hiến máu để cập nhật.";
                    return Page();
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật trạng thái.";
                return Page();
            }
        }

        // Additional methods for admin features
        public async Task<IActionResult> OnGetByUserAsync(int userId)
        {
            try
            {
                var userDonations = await _donationHistoryService.GetByUserIdAsync(userId);
                // Handle user donations display
                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu người dùng.";
                return Page();
            }
        }

        public async Task<IActionResult> OnGetByStatusAsync(DonationStatus status)
        {
            try
            {
                var statusDonations = await _donationHistoryService.GetByStatusAsync(status);
                // Handle status-based display
                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu theo trạng thái.";
                return Page();
            }
        }
    }
}
