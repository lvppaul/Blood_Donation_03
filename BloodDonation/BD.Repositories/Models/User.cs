using System;
using System.Collections.Generic;

namespace BD.Repositories.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string BloodType { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();

    public virtual ICollection<DonationHistory> DonationHistories { get; set; } = new List<DonationHistory>();

    public virtual ICollection<DonorAvailability> DonorAvailabilities { get; set; } = new List<DonorAvailability>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Role Role { get; set; } = null!;
}
