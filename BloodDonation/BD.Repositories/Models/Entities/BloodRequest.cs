using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class BloodRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public string BloodType { get; set; } = null!;

    public string ComponentType { get; set; } = null!;

    public int Quantity { get; set; }

    public string UrgencyLevel { get; set; } = null!;

    public DateTime? RequestDate { get; set; }

    public int StatusRequestId { get; set; }

    public DateTime? FulfilledDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();

    public virtual StatusBloodRequest StatusRequest { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
