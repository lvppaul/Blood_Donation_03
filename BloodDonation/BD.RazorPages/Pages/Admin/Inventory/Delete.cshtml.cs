using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Admin.Inventory
{
    public class DeleteModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public DeleteModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
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

            var bloodinventory = await _context.BloodInventories.FirstOrDefaultAsync(m => m.InventoryId == id);

            if (bloodinventory == null)
            {
                return NotFound();
            }
            else
            {
                BloodInventory = bloodinventory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodinventory = await _context.BloodInventories.FindAsync(id);
            if (bloodinventory != null)
            {
                BloodInventory = bloodinventory;
                _context.BloodInventories.Remove(BloodInventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
