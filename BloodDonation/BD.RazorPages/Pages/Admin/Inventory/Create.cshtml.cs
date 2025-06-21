using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class CreateModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public CreateModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FacilityId"] = new SelectList(_context.MedicalFacilities, "FacilityId", "Address");
        ViewData["StatusInventoryId"] = new SelectList(_context.StatusBloodInventories, "StatusInventoryId", "StatusName");
            return Page();
        }

        [BindProperty]
        public BloodInventory BloodInventory { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BloodInventories.Add(BloodInventory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
