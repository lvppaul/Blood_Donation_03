using BD.Repositories.Models.Entities;

namespace BD.RazorPages.Models
{
    public class DonationStatusViewModel
    {
        public int DonationId { get; set; }
        public string DonorName { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;
        public DonationStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? DonationDate { get; set; }

        public static DonationStatusViewModel FromDonationHistory(DonationHistory donation)
        {
            return new DonationStatusViewModel
            {
                DonationId = donation.DonationId,
                DonorName = donation.User?.Name ?? "Unknown",
                BloodType = donation.BloodType,
                Status = donation.Status,
                CreatedDate = donation.CreatedDate,
                ConfirmedDate = donation.ConfirmedDate,
                DonationDate = donation.DonationDate
            };
        }
    }
}
