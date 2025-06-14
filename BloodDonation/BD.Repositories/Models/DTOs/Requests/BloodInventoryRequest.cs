using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class BloodInventoryRequest
    {
        public int FacilityId { get; set; }
        public string ComponentType { get; set; } = null!;
        public int Amount { get; set; }
        public DateOnly ExpiredDate { get; set; }
        public int StatusInventoryId { get; set; }
        public string BloodType { get; set; } = null!;
    }
}
