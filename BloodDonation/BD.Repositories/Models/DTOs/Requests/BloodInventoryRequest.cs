using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class BloodInventoryRequest
    {
        [Required(ErrorMessage = "Medical facility is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid medical facility")]
        public int FacilityId { get; set; }

        [Required(ErrorMessage = "Component type is required")]
        [StringLength(50, ErrorMessage = "Component type cannot exceed 50 characters")]
        public string ComponentType { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 1000, ErrorMessage = "Amount must be between 1 and 1000 units")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        public DateOnly ExpiredDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid status")]
        public int StatusInventoryId { get; set; }

        [Required(ErrorMessage = "Blood type is required")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "Please select a valid blood type")]
        [StringLength(10, ErrorMessage = "Blood type cannot exceed 10 characters")]
        public string BloodType { get; set; } = null!;
    }
}
