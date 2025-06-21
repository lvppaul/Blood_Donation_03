using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages
{
    public class IndexModel : PageModel
    {        private readonly ILogger<IndexModel> _logger;
        private readonly IBloodRequestRepository _bloodRequestRepository;
        private readonly IDonorAvailabilityRepository _donorAvailabilityRepository;
        private readonly IBloodInventoryRepository _bloodInventoryRepository;
        private readonly IMedicalFacilityRepository _medicalFacilityRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public IndexModel(ILogger<IndexModel> logger, 
            IBloodRequestRepository bloodRequestRepository,
            IDonorAvailabilityRepository donorAvailabilityRepository,
            IBloodInventoryRepository bloodInventoryRepository,
            IMedicalFacilityRepository medicalFacilityRepository,
            IBlogPostRepository blogPostRepository)
        {
            _logger = logger;
            _bloodRequestRepository = bloodRequestRepository;
            _donorAvailabilityRepository = donorAvailabilityRepository;
            _bloodInventoryRepository = bloodInventoryRepository;
            _medicalFacilityRepository = medicalFacilityRepository;
            _blogPostRepository = blogPostRepository;
        }        // Properties to hold data for the view
        public int TotalBloodRequests { get; set; }
        public int AvailableDonors { get; set; }
        public int TotalBloodInventory { get; set; }
        public int MedicalFacilities { get; set; }
        public IEnumerable<BloodRequest> RecentRequests { get; set; } = new List<BloodRequest>();
        public IEnumerable<BlogPost> LatestBlogPosts { get; set; } = new List<BlogPost>();

        public async Task OnGetAsync()
        {
            try
            {
                // Get statistics for the dashboard
                var allRequests = await _bloodRequestRepository.GetAllBloodRequestsAsync();
                TotalBloodRequests = allRequests.Count();

                var availableDonors = await _donorAvailabilityRepository.GetAllDonorAvailabilitiesAsync();
                AvailableDonors = availableDonors.Count();

                var bloodInventory = await _bloodInventoryRepository.GetAllBloodInventoriesAsync();
                TotalBloodInventory = bloodInventory.Sum(bi => bi.Amount);

                var facilities = await _medicalFacilityRepository.GetAllFacilitiesAsync();
                MedicalFacilities = facilities.Count();                // Get recent blood requests (top 5)
                RecentRequests = allRequests.OrderByDescending(r => r.RequestDate).Take(1);
                var allBlogPosts = await _blogPostRepository.GetAllPostsAsync();
                LatestBlogPosts = allBlogPosts
                    .Where(bp => bp.IsDeleted != true)
                    .OrderByDescending(bp => bp.CreatedAt)
                    .Take(3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");                // Set default values if there's an error
                TotalBloodRequests = 0;
                AvailableDonors = 0;
                TotalBloodInventory = 0;
                MedicalFacilities = 0;
                RecentRequests = new List<BloodRequest>();
                LatestBlogPosts = new List<BlogPost>();
            }
        }
    }
}
