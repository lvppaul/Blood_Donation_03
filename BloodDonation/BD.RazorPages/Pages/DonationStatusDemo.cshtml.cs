using Microsoft.AspNetCore.Mvc.RazorPages;
using BD.Repositories.Models.Entities;
using BD.RazorPages.Models;

namespace BD.RazorPages.Pages
{
    public class DonationStatusDemoModel : PageModel
    {
        public DonationStatus[] AllStatuses { get; set; } = new[]
        {
            DonationStatus.Waiting,
            DonationStatus.Confirmed,
            DonationStatus.Donated
        };

        public void OnGet()
        {
            // Demo page - no special logic needed
        }

        public DonationStatusViewModel CreateDemoViewModel(DonationStatus status)
        {
            return new DonationStatusViewModel
            {
                DonationId = 1,
                DonorName = "Demo User",
                BloodType = "A+",
                Status = status,
                CreatedDate = DateTime.Now.AddDays(-5),
                ConfirmedDate = status >= DonationStatus.Confirmed ? DateTime.Now.AddDays(-2) : null,
                DonationDate = status >= DonationStatus.Donated ? DateTime.Now : null
            };
        }
    }
}
