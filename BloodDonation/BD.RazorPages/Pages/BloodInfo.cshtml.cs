using BD.Repositories.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BD.RazorPages.Pages
{
    public class BloodInfoModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public BloodInfoModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        public List<string> AllBloodTypes { get; set; } = new List<string>
        {
            "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };

        public List<string> AllComponentTypes { get; set; } = new List<string>
        {
            "Red Blood Cells", "Plasma", "Platelets", "White Blood Cells"
        };

        [BindProperty]
        public string RecipientBloodType { get; set; } = "";

        [BindProperty]
        public string ComponentType { get; set; } = "Red Blood Cells";

        [BindProperty]
        public string SearchMode { get; set; } = "WholeBlood";

        public List<BloodTypeDetail> DonorMatches { get; set; } = new();

        public bool HasSearched { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
            HasSearched = true;

            if (string.IsNullOrEmpty(RecipientBloodType))
                return Page();

            var query = _context.BloodCompatibilities
                .Where(bc =>
                    bc.RecipientBloodType == RecipientBloodType &&
                    bc.IsCompatible == true &&
                    (bc.IsDeleted == false || bc.IsDeleted == null)
                );

            if (SearchMode == "Component" && !string.IsNullOrEmpty(ComponentType))
            {
                query = query.Where(bc => bc.ComponentType == ComponentType);
            }

            var results = await query
                .Select(bc => bc.DonorBloodType)
                .Distinct()
                .ToListAsync();

            DonorMatches = results.Select(b => new BloodTypeDetail
            {
                Name = b,
                Description = "Compatible Blood Type",
                Population = GetPopulation(b)
            }).ToList();

            return Page();
        }

        private string GetPopulation(string bloodType)
        {
            return bloodType switch
            {
                "O+" => "37% of population",
                "A+" => "36% of population",
                "B+" => "8% of population",
                "AB+" => "3% of population",
                _ => ""
            };
        }
    }

    public class BloodTypeDetail
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Population { get; set; } = "";
    }
}
