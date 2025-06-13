using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class MedicalFacilityResponse
    {
        public int FacilityId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }
    }
}
