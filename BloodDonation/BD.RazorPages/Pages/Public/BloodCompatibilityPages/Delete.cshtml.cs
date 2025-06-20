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
    public class DeleteModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public DeleteModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodcompatibility = await _context.BloodCompatibilities.FindAsync(id);
            if (bloodcompatibility != null)
            {
                BloodCompatibility = bloodcompatibility;
                bloodcompatibility.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
