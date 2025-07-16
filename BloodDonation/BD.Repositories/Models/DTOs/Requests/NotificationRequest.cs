namespace BD.Repositories.Models.DTOs.Requests
{
    public class NotificationRequest
    {
        public int UserId { get; set; }
        public string Message { get; set; } = null!;
        public string? Type { get; set; }
        public DateTime? SentAt { get; set; }
        public int StatusNotificationId { get; set; }

    }
}
