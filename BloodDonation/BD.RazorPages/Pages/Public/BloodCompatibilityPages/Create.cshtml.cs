using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Public.BloodCompatibilityPages
{
    public class CreateModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public CreateModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BloodCompatibility BloodCompatibility { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BloodCompatibilities.Add(BloodCompatibility);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
