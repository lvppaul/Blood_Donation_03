namespace BD.Repositories.Models.DTOs.Responses
{
    public class BloodRequestResponse
    {
        public int RequestId { get; set; }
        //public int UserId { get; set; }
        //public int StatusRequestId { get; set; }

        public string BloodType { get; set; } = null!;

        public string ComponentType { get; set; } = null!;

        public int Amount { get; set; }

        public string UrgencyLevel { get; set; } = null!;

        public DateTime? RequestDate { get; set; }


        public DateTime? FulfilledDate { get; set; }

        public bool? IsDeleted { get; set; }

        public UserResponse User { get; set; } = null!;

        public StatusBloodRequestResponse StatusBloodRequestResponse { get; set; } = null!;
    }
}
