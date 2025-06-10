using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class DonorAvailability
{
    public int AvailabilityId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public DateOnly? LastDonationDate { get; set; }

    public DateOnly? RecoveryReminderDate { get; set; }

    public DateOnly AvailableDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual StatusesBloodDonor Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
