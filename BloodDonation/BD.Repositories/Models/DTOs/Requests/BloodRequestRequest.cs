using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class BloodRequestRequest
    {
        public int UserId { get; set; }

        public string BloodType { get; set; } = null!;

        public string ComponentType { get; set; } = null!;

        public int Amount { get; set; }

        public string UrgencyLevel { get; set; } = null!;

        public DateTime? RequestDate { get; set; }

        public int StatusRequestId { get; set; }

        public DateTime? FulfilledDate { get; set; }
    }

}
