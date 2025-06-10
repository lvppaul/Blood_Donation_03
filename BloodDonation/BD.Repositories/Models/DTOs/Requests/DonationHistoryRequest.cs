using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class DonationHistoryRequest
    {
        public int RequestId { get; set; }
        public int FacilityId { get; set; }
        public int Amount { get; set; }
        public DateTime? DonationDate { get; set; }
        public string BloodType { get; set; } = null!;
        public string ComponentType { get; set; } = null!;
    }
}
