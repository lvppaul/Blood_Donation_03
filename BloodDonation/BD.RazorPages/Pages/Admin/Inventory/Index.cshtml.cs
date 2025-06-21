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
    public class IndexModel : PageModel
    {
        private readonly BD.Repositories.Models.Entities.BloodDonationDbContext _context;

        public IndexModel(BD.Repositories.Models.Entities.BloodDonationDbContext context)
        {
            _context = context;
        }

        public IList<BloodInventory> BloodInventory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            BloodInventory = await _context.BloodInventories
                .Include(b => b.Facility)
                .Include(b => b.StatusInventory).ToListAsync();
        }
    }
}
