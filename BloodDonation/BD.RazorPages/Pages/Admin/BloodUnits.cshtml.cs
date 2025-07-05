using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Services.Interfaces;
using BD.Services.Others;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BD.RazorPages.Pages.Admin
{
    public class BloodUnitsModel : PageModel
    {
        private IBloodInventoryService _bloodInventory;
        public IEnumerable<BloodInventoryResponse> BloodUnits { get; set; }
        public IEnumerable<BloodTypeOverview> BloodTypeOverviews { get; set; }

        public int TotalCountA { get; set; }
        public BloodUnitsModel(IBloodInventoryService bloodInventory)
        {
            _bloodInventory = bloodInventory;
        }
        public async Task OnGetAsync()
        {
            BloodUnits = await _bloodInventory.GetAllAsync();
            BloodTypeOverviews = await _bloodInventory.GetBloodTypeOverviewAsync();
        }
    }

}
