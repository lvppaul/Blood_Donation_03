using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class StatusBloodDonor
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<DonorAvailability> DonorAvailabilities { get; set; } = new List<DonorAvailability>();
}
