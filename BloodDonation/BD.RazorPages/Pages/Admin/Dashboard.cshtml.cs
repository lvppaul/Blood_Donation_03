using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Services.Interfaces;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.RazorPages.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDonorAvailabilityService _donorAvailabilityService;
        private readonly IDonationHistoryService _donationHistoryService;
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IBloodRequestService _bloodRequestService;

        public DashboardModel(
            IUserService userService,
            IDonorAvailabilityService donorAvailabilityService,
            IDonationHistoryService donationHistoryService,
            IBloodInventoryService bloodInventoryService,
            IBloodRequestService bloodRequestService)
        {
            _userService = userService;
            _donorAvailabilityService = donorAvailabilityService;
            _donationHistoryService = donationHistoryService;
            _bloodInventoryService = bloodInventoryService;
            _bloodRequestService = bloodRequestService;
        }

        // Dashboard Statistics
        public int TotalUsers { get; set; }
        public int ActiveDonors { get; set; }
        public int TotalBloodUnits { get; set; }
        public int PendingRequests { get; set; }

        // Recent Data
        public IEnumerable<DonationHistoryResponse> RecentDonations { get; set; } = new List<DonationHistoryResponse>();
        public IEnumerable<BloodRequestResponse> RecentRequests { get; set; } = new List<BloodRequestResponse>();

        // Chart Data
        public Dictionary<string, int> BloodTypeDistribution { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> MonthlyDonations { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> BloodInventoryByType { get; set; } = new Dictionary<string, int>();

        public async Task OnGetAsync()
        {
            try
            {
                // Get all data with null checks and error handling
                var donorAvailabilities = await SafeGetDataAsync(() => _donorAvailabilityService.GetAllAvailableDonorsAsync()) ?? Enumerable.Empty<DonorAvailabilityResponse>();
                var users = await SafeGetDataAsync(() => _userService.GetAllAsync()) ?? Enumerable.Empty<UserResponse>();
                var donations = await SafeGetDataAsync(() => _donationHistoryService.GetAllAsync()) ?? Enumerable.Empty<DonationHistoryResponse>();
                var bloodInventory = await SafeGetDataAsync(() => _bloodInventoryService.GetAllAsync()) ?? Enumerable.Empty<BloodInventoryResponse>();
                var bloodRequests = await SafeGetDataAsync(() => _bloodRequestService.GetAllAsync()) ?? Enumerable.Empty<BloodRequestResponse>();

                // Calculate statistics with null checks
                TotalUsers = users?.Count() ?? 0;
                ActiveDonors = donorAvailabilities?.Select(da => da.User?.UserId).Distinct().Count() ?? 0;
                TotalBloodUnits = bloodInventory?.Sum(b => b?.Amount ?? 0) ?? 0;
                PendingRequests = bloodRequests?.Count(r => IsPendingRequest(r)) ?? 0;

                // Recent donations (last 10) with null checks
                RecentDonations = donations?
                    .Where(d => d != null)
                    .OrderByDescending(d => d.DonationDate ?? DateTime.MinValue)
                    .Take(10) ?? Enumerable.Empty<DonationHistoryResponse>();

                // Recent requests (last 10) with null checks
                RecentRequests = bloodRequests?
                    .Where(r => r != null)
                    .OrderByDescending(r => r.RequestDate ?? DateTime.MinValue)
                    .Take(10) ?? Enumerable.Empty<BloodRequestResponse>();

                // Blood type distribution from users with null checks
                BloodTypeDistribution = users?
                    .Where(u => !string.IsNullOrEmpty(u?.BloodType))
                    .GroupBy(u => u.BloodType)
                    .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();

                // Monthly donations (last 6 months) with null checks
                var sixMonthsAgo = DateTime.Now.AddMonths(-6);
                MonthlyDonations = donations?
                    .Where(d => d?.DonationDate != null && d.DonationDate >= sixMonthsAgo)
                    .GroupBy(d => d.DonationDate?.ToString("yyyy-MM") ?? "Unknown")
                    .Where(g => !string.IsNullOrEmpty(g.Key) && g.Key != "Unknown")
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();

                // Blood inventory by type with null checks
                BloodInventoryByType = bloodInventory?
                    .Where(b => b != null && !string.IsNullOrEmpty(b.BloodType))
                    .GroupBy(b => b.BloodType)
                    .ToDictionary(g => g.Key, g => g.Sum(b => b?.Amount ?? 0)) ?? new Dictionary<string, int>();

                // Ensure we have some default data for charts if everything is empty
                EnsureMinimumChartData();
            }
            catch (Exception ex)
            {
                // Handle any errors gracefully with logging
                Console.WriteLine($"Error in Dashboard OnGetAsync: {ex.Message}");
                
                // Set default values
                SetDefaultValues();
            }
        }

        private async Task<IEnumerable<T>?> SafeGetDataAsync<T>(Func<Task<IEnumerable<T>>> dataFunction)
        {
            try
            {
                return await dataFunction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data: {ex.Message}");
                return null;
            }
        }

        // private static bool IsUserDonor(UserResponse? user)
        // {
        //     return user?.Role?.Name?.ToLower() is "member" or "donor";
        // }

        private static bool IsPendingRequest(BloodRequestResponse? request)
        {
            return request?.StatusBloodRequestResponse?.StatusName?.ToLower() == "pending";
        }

        private void SetDefaultValues()
        {
            TotalUsers = 0;
            ActiveDonors = 0;
            TotalBloodUnits = 0;
            PendingRequests = 0;
            RecentDonations = Enumerable.Empty<DonationHistoryResponse>();
            RecentRequests = Enumerable.Empty<BloodRequestResponse>();
            BloodTypeDistribution = new Dictionary<string, int>();
            MonthlyDonations = new Dictionary<string, int>();
            BloodInventoryByType = new Dictionary<string, int>();
            EnsureMinimumChartData();
        }

        private void EnsureMinimumChartData()
        {
            // Provide default chart data if empty to prevent JavaScript errors
            if (!BloodTypeDistribution.Any())
            {
                BloodTypeDistribution = new Dictionary<string, int>
                {
                    ["No Data"] = 1
                };
            }

            if (!MonthlyDonations.Any())
            {
                MonthlyDonations = new Dictionary<string, int>
                {
                    [DateTime.Now.ToString("yyyy-MM")] = 0
                };
            }

            if (!BloodInventoryByType.Any())
            {
                BloodInventoryByType = new Dictionary<string, int>
                {
                    ["No Data"] = 0
                };
            }
        }
    }
}
