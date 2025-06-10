using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class MedicalFacility
{
    public int FacilityId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();

    public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();
}
