using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class BloodInventory
{
    public int InventoryId { get; set; }

    public int FacilityId { get; set; }

    public string ComponentType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public bool? IsDeleted { get; set; }

    public string BloodType { get; set; } = null!;

    public virtual MedicalFacility Facility { get; set; } = null!;
}
