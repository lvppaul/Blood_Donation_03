﻿using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class DonationHistory
{
    public int DonationId { get; set; }

    public int UserId { get; set; }

    public int RequestId { get; set; }

    public int FacilityId { get; set; }

    public int Quantity { get; set; }

    public DateTime? DonationDate { get; set; }

    public string BloodType { get; set; } = null!;

    public string ComponentType { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual MedicalFacility Facility { get; set; } = null!;

    public virtual BloodRequest Request { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
