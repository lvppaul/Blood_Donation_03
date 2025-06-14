namespace BD.Repositories.Models.DTOs.Responses
{
    public class StatusNotificationResponse
    {
        public int StatusNotificationId { get; set; }
        public string StatusName { get; set; } = null!;
        public bool? IsDeleted { get; set; }

    }
}
