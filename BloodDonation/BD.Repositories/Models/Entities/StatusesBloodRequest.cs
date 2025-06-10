using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class StatusesBloodRequest
{
    public int StatusRequestId { get; set; }

    public string StatusName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
}
