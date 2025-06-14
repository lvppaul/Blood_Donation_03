using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class NotificationMapper
    {
        public static Notification ToEntity(NotificationRequest request)
        {
            return new Notification
            {
                UserId = request.UserId,
                Message = request.Message,
                Type = request.Type,
                SentAt = request.SentAt,
                StatusNotificationId = request.StatusNotificationId,
                IsDeleted = false
            };
        }

        public static NotificationResponse ToResponseDto(Notification entity)
        {
            return new NotificationResponse
            {
                NotificationId = entity.NotificationId,
                UserId = entity.UserId,
                Message = entity.Message,
                Type = entity.Type,
                SentAt = entity.SentAt,
                StatusNotificationId = entity.StatusNotificationId,
                // StatusName = entity.StatusNotification?.StatusName ?? string.Empty,
                //IsDeleted = entity.IsDeleted,
                // UserName = entity.User?.Name ?? string.Empty
            };
        }

    }
}
