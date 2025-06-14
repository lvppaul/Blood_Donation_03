namespace BD.Repositories.Models.DTOs.Responses
{
    public class BloodCompatibilityResponse
    {
        public int CompatibilityId { get; set; }
        public string RecipientBloodType { get; set; } = null!;
        public string DonorBloodType { get; set; } = null!;
        public string ComponentType { get; set; } = null!;
        public bool IsCompatible { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
