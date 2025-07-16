using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Member
{
    public class DeleteModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public DeleteModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BloodRequest BloodRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodrequest = await _context.BloodRequests.FirstOrDefaultAsync(m => m.RequestId == id);

            if (bloodrequest == null)
            {
                return NotFound();
            }
            else
            {
                BloodRequest = bloodrequest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodrequest = await _context.BloodRequests.FindAsync(id);
            if (bloodrequest != null)
            {
                BloodRequest = bloodrequest;
                _context.BloodRequests.Remove(BloodRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
