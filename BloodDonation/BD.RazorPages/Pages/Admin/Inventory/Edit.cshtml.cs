using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class EditModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public EditModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BloodInventory BloodInventory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodinventory =  await _context.BloodInventories.FirstOrDefaultAsync(m => m.InventoryId == id);
            if (bloodinventory == null)
            {
                return NotFound();
            }
            BloodInventory = bloodinventory;
           ViewData["FacilityId"] = new SelectList(_context.MedicalFacilities, "FacilityId", "Address");
           ViewData["StatusInventoryId"] = new SelectList(_context.StatusBloodInventories, "StatusInventoryId", "StatusName");
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

            _context.Attach(BloodInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloodInventoryExists(BloodInventory.InventoryId))
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

        private bool BloodInventoryExists(int id)
        {
            return _context.BloodInventories.Any(e => e.InventoryId == id);
        }
    }
}
