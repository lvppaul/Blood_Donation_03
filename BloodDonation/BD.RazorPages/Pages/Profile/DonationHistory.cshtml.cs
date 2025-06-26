using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.RazorPages.Pages.Profile
{
    public class DonationHistoryModel : PageModel
    {
        private readonly IDonationHistoryRepository _donationHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DonationHistoryModel> _logger;

        public DonationHistoryModel(
            IDonationHistoryRepository donationHistoryRepository,
            IUserRepository userRepository,
            ILogger<DonationHistoryModel> logger)
        {
            _donationHistoryRepository = donationHistoryRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public string UserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public List<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
        public int TotalDonations { get; set; }
        public DateTime? LastDonationDate { get; set; }
        public DateTime? NextEligibleDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Login");
            }

            UserName = HttpContext.Session.GetString("UserName") ?? "Unknown User";
            UserRole = HttpContext.Session.GetString("UserRole") ?? "guest";

            try
            {
                await LoadDonationHistoryAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading donation history for user {UserId}", userId);
                // Continue with empty data rather than failing
            }

            return Page();
        }

        private async Task LoadDonationHistoryAsync(string userId)
        {

            // Load real user donation history from database
            if (int.TryParse(userId, out int userIdInt))
            {
                try
                {
                    // Get donation histories for the user, including related entities
                    var donations = await _donationHistoryRepository.GetByUserIdAsync(userIdInt);
                    
                    DonationHistories = donations.ToList();
                    TotalDonations = DonationHistories.Count;
                    
                    if (DonationHistories.Any())
                    {
                        LastDonationDate = DonationHistories.OrderByDescending(d => d.DonationDate).First().DonationDate;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error querying donation history for user {UserId}", userIdInt);
                    // Fall back to demo data
                }
            }
          
        }

    }
}
