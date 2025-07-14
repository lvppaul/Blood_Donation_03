using System.ComponentModel.DataAnnotations;

namespace BD.RazorPages.Models
{
    public class BloodDonorSearchRequest
    {
        [Display(Name = "Blood Type")]
        public string BloodType { get; set; } = string.Empty;

        [Display(Name = "Search Type")]
        public string SearchType { get; set; } = "compatible";
    }
}
