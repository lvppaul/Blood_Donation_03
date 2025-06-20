using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Public.BloodCompatibilityPages
{
    public class EditModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public EditModel(BloodDonationDbContext context)
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

            var bloodcompatibility =  await _context.BloodCompatibilities.FirstOrDefaultAsync(m => m.CompatibilityId == id);
            if (bloodcompatibility == null)
            {
                return NotFound();
            }
            BloodCompatibility = bloodcompatibility;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BloodCompatibility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloodCompatibilityExists(BloodCompatibility.CompatibilityId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BloodCompatibilityExists(int id)
        {
            return _context.BloodCompatibilities.Any(e => e.CompatibilityId == id);
        }
    }
}
