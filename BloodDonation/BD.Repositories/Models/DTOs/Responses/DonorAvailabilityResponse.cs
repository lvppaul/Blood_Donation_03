using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class DonorAvailabilityResponse
    {
        public int AvailabilityId { get; set; }

        public DateOnly? LastDonationDate { get; set; }

        public DateOnly? RecoveryReminderDate { get; set; }

        public DateOnly AvailableDate { get; set; }

        public bool? IsDeleted { get; set; }

        public UserResponse User { get; set; } = null!;

        public StatusBloodDonorResponse StatusDonor { get; set; } = null!;
    }
}
