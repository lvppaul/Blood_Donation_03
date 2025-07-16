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
    public class DetailsModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public DetailsModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

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
    }
}
