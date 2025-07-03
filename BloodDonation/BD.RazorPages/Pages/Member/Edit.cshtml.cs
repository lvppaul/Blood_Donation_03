using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Member
{
    public class EditModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public EditModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
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

            var bloodrequest =  await _context.BloodRequests.FirstOrDefaultAsync(m => m.RequestId == id);
            if (bloodrequest == null)
            {
                return NotFound();
            }
            BloodRequest = bloodrequest;
           ViewData["StatusRequestId"] = new SelectList(_context.StatusBloodRequests, "StatusRequestId", "StatusName");
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "BloodType");
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

            _context.Attach(BloodRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloodRequestExists(BloodRequest.RequestId))
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

        private bool BloodRequestExists(int id)
        {
            return _context.BloodRequests.Any(e => e.RequestId == id);
        }
    }
}
