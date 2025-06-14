using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class StatusNotificationMapper
    {
        public static StatusNotification ToEntity(StatusNotificationRequest request)
        {
            return new StatusNotification
            {
                StatusName = request.StatusName,
                IsDeleted = false
            };
        }

        public static StatusNotificationResponse ToResponseDto(StatusNotification entity)
        {
            return new StatusNotificationResponse
            {
                StatusNotificationId = entity.StatusNotificationId,
                StatusName = entity.StatusName,
                IsDeleted = entity.IsDeleted
            };
        }

    }
}
