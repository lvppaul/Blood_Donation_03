using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class StatusBloodInventory
{
    public int StatusInventoryId { get; set; }

    public string StatusName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();
}
