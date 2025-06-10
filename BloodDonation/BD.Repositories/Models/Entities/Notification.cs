using System;
using System.Collections.Generic;

namespace BD.Repositories.Models.Entities;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public string? Type { get; set; }

    public DateTime? SentAt { get; set; }

    public int StatusNotificationId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual StatusNotification StatusNotification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
