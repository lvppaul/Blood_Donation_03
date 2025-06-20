using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Public.BloodCompatibilityPages
{
    public class SearchModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public SearchModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string RecipientBloodType { get; set; } = string.Empty;

        [BindProperty]
        public string ComponentType { get; set; } = "Red Blood Cells"; // default

        [BindProperty]
        public string SearchMode { get; set; } = "All";

        public List<string> DonorMatches { get; set; } = new();
        public bool HasSearched { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
            HasSearched = true;

            if (string.IsNullOrEmpty(RecipientBloodType))
                return Page();

            IQueryable<BloodCompatibility> query = _context.BloodCompatibilities
                .Where(bc =>
                    bc.RecipientBloodType == RecipientBloodType &&
                    bc.IsCompatible == true &&
                    (bc.IsDeleted == false || bc.IsDeleted == null));

            if (SearchMode == "Component" && !string.IsNullOrEmpty(ComponentType))
            {
                query = query.Where(bc => bc.ComponentType == ComponentType);
            }

            DonorMatches = await query
                .Select(bc => bc.DonorBloodType)
                .Distinct()
                .ToListAsync();

            return Page();
        }
    }
}
