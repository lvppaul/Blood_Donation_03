namespace BD.Repositories.Models.DTOs.Requests
{
    public class BloodCompatibilityRequest
    {
        public string RecipientBloodType { get; set; } = null!;
        public string DonorBloodType { get; set; } = null!;
        public string ComponentType { get; set; } = null!;
        public bool IsCompatible { get; set; }
    }
}
