using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Pages.Member
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
        ViewData["StatusRequestId"] = new SelectList(_context.StatusBloodRequests, "StatusRequestId", "StatusName");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "BloodType");
            return Page();
        }

        [BindProperty]
        public BloodRequest BloodRequest { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BloodRequests.Add(BloodRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
