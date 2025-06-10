using System;
using System.Collections.Generic;

namespace BD.Repositories.Models;

public partial class StatusNotification
{
    public int StatusNotificationId { get; set; }

    public string StatusName { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
