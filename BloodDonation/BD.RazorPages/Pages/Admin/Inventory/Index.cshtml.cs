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
    public class IndexModel : PageModel
    {
        private readonly BloodDonationDbContext _context;

        public IndexModel(BloodDonationDbContext context)
        {
            _context = context;
        }

        public IList<BloodInventory> BloodInventory { get; set; } = new List<BloodInventory>();

        public List<SelectListItem> StatusSelectList { get; set; } = new();

        // ✅ Bind Search và Status để dùng trong Razor Page (GET)
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }

        public async Task OnGetAsync()
        {
            // Load danh sách status cho dropdown
            StatusSelectList = await _context.StatusBloodInventories
                .Where(s => s.IsDeleted == false)
                .Select(s => new SelectListItem
                {
                    Value = s.StatusInventoryId.ToString(),
                    Text = s.StatusName,
                    Selected = Status.HasValue && Status.Value == s.StatusInventoryId
                })
                .ToListAsync();

            // Truy vấn BloodInventory
            var query = _context.BloodInventories
                .Include(b => b.Facility)
                .Include(b => b.StatusInventory)
                .Where(b => b.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(b =>
                    b.BloodType.Contains(Search) ||
                    b.ComponentType.Contains(Search) ||
                    b.Facility.Name.Contains(Search));
            }

            if (Status.HasValue)
            {
                query = query.Where(b => b.StatusInventoryId == Status.Value);
            }

            BloodInventory = await query.ToListAsync();
        }
    }
}
