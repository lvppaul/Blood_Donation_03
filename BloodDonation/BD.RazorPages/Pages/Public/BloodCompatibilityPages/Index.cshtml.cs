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
    public class IndexModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public IndexModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        public IList<BloodCompatibility> BloodCompatibility { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.BloodCompatibilities.AsQueryable();

            if (Filter == "Red Blood Cells")
            {
                query = query.Where(b => b.ComponentType == "Red Blood Cells");
            }
            else if (Filter == "Plasma")
            {
                query = query.Where(b => b.ComponentType == "Plasma");
            }
            else if (Filter == "Platelets")
            {
                query = query.Where(b => b.ComponentType == "Platelets");
            }
            else if (Filter == "White Blood Cells")
            {
                query = query.Where(b => b.ComponentType == "White Blood Cells");
            }

            BloodCompatibility = await query.ToListAsync();
        }
    }
}
