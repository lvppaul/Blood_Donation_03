using BD.Repositories.Models.DTOs.Responses;
using BD.Services.Interfaces;
using BD.Services.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BD.RazorPages.Pages.Admin
{
    public class BloodUnitsModel : PageModel
    {
        private readonly IBloodInventoryService _bloodInventoryService;
        private readonly IMedicalFacilityService _medicalFacilityService;

        public IEnumerable<BloodInventoryResponse> BloodUnits { get; set; } = Enumerable.Empty<BloodInventoryResponse>();
        public IEnumerable<BloodTypeOverview> BloodTypeOverviews { get; set; } = Enumerable.Empty<BloodTypeOverview>();
        public IEnumerable<MedicalFacilityResponse> MedicalFacilities { get; set; } = Enumerable.Empty<MedicalFacilityResponse>();

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalBloodUnits { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalBloodUnits / PageSize);

        // Filter properties
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public string SelectedBloodType { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public int? SelectedFacilityId { get; set; }

        public BloodUnitsModel(IBloodInventoryService bloodInventoryService, IMedicalFacilityService medicalFacilityService)
        {
            _bloodInventoryService = bloodInventoryService;
            _medicalFacilityService = medicalFacilityService;
        }

        public async Task OnGetAsync()
        {
            if (CurrentPage < 1) CurrentPage = 1;
            if (!new[] { 10, 25, 50 }.Contains(PageSize)) PageSize = 10;

            try
            {
                (var units, TotalBloodUnits) = await _bloodInventoryService.GetFilteredAsync(
                    SearchTerm, SelectedBloodType, SelectedFacilityId, CurrentPage, PageSize);
                BloodUnits = units ?? Enumerable.Empty<BloodInventoryResponse>();

                BloodTypeOverviews = await _bloodInventoryService.GetBloodTypeOverviewAsync() ?? Enumerable.Empty<BloodTypeOverview>();
                MedicalFacilities = await _medicalFacilityService.GetAllAsync() ?? Enumerable.Empty<MedicalFacilityResponse>();

                if (CurrentPage > TotalPages && TotalPages > 0)
                {
                    CurrentPage = TotalPages;
                    (BloodUnits, TotalBloodUnits) = await _bloodInventoryService.GetFilteredAsync(
                        SearchTerm, SelectedBloodType, SelectedFacilityId, CurrentPage, PageSize);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading blood units. Please try again later.";
                Console.WriteLine($"Error in BloodUnits OnGetAsync: {ex.Message}");
                BloodUnits = Enumerable.Empty<BloodInventoryResponse>();
                BloodTypeOverviews = Enumerable.Empty<BloodTypeOverview>();
                MedicalFacilities = Enumerable.Empty<MedicalFacilityResponse>();
                TotalBloodUnits = 0;
            }
        }
    }
}
