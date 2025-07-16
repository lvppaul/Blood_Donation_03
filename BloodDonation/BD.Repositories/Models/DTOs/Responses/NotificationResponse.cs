namespace BD.Repositories.Models.DTOs.Responses
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;
        public string? Type { get; set; }
        public DateTime? SentAt { get; set; }
        public int StatusNotificationId { get; set; }
        //public string StatusName { get; set; } = null!;
        public bool? IsDeleted { get; set; }
        //public string UserName { get; set; } = null!;

    }
}
