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

        public UserResponse UserId { get; set; } = null!;

        public MedicalFacilityResponse Facility { get; set; } = null!;
    }
}
