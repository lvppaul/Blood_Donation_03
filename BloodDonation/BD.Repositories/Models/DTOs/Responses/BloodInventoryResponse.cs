using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class BloodInventoryResponse
    {
        public int InventoryId { get; set; }
        public string ComponentType { get; set; } = null!;
        public int Amount { get; set; }
        public DateOnly ExpiredDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string BloodType { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public StatusBloodInventoryResponse StatusBloodInventory { get; set; } = null!;
        public MedicalFacilityResponse Facility { get; set; } = null!;
    }
}
