using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Public.BloodCompatibilityPages
{
    public class DetailsModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public DetailsModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

        public BloodCompatibility BloodCompatibility { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodcompatibility = await _context.BloodCompatibilities.FirstOrDefaultAsync(m => m.CompatibilityId == id);
            if (bloodcompatibility == null)
            {
                return NotFound();
            }
            else
            {
                BloodCompatibility = bloodcompatibility;
            }
            return Page();
        }
    }
}
