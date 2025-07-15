using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class DonationHistoryResponse
    {
        public int DonationId { get; set; }

        public int Amount { get; set; }

        public DateTime? DonationDate { get; set; }

        public string BloodType { get; set; } = null!;

        public string ComponentType { get; set; } = null!;

        public int DonationRequest { get; set; }

        public bool IsDeleted { get; set; }

        public Entities.DonationStatus Status { get; set; } = Entities.DonationStatus.Waiting;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ConfirmedDate { get; set; }

        public UserResponse User { get; set; } = null!;

        public MedicalFacilityResponse Facility { get; set; } = null!;
    }
}
