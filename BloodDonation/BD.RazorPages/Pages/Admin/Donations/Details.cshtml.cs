using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using BD.RazorPages.Models;

namespace BD.RazorPages.Pages.Admin.Donations
{
    public class DetailsModel : PageModel
    {
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IUserService _userService;

        public DetailsModel(IDonationHistoryService donationHistoryService, IUserService userService)
        {
            _donationHistoryService = donationHistoryService;
            _userService = userService;
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
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin donation.";
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
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi tải thông tin donation: {ex.Message}";
                return RedirectToPage("/Admin/Users/Donors", new { ActiveTab = "donations" });
            }
        }

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

            return RedirectToPage(new { id = donationId });
        }
    }
}
