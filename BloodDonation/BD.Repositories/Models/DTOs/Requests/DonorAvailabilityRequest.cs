using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class DonorAvailabilityRequest
    {
        public int UserId { get; set; }

        public int StatusDonorId { get; set; }

        public DateOnly? LastDonationDate { get; set; }

        public DateOnly? RecoveryReminderDate { get; set; }

        public DateOnly AvailableDate { get; set; }
    }
}
