using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class StatusNotificationService : IStatusNotificationService
    {
        private readonly IStatusNotificationRepository _statusNotificationRepository;
        public StatusNotificationService(IStatusNotificationRepository statusNotificationRepository)
        {
            _statusNotificationRepository = statusNotificationRepository;
        }
        public async Task AddAsync(StatusNotificationRequest statusNotificationRequest)
        {
            var notificationEntity = StatusNotificationMapper.ToEntity(statusNotificationRequest);
            await _statusNotificationRepository.AddAsync(notificationEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _statusNotificationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StatusNotificationResponse>> GetAllAsync()
        {
            var statusNotifications = await _statusNotificationRepository.GetAllAsync();
            return statusNotifications.Select(StatusNotificationMapper.ToResponseDto);
        }

        public async Task<StatusNotificationResponse?> GetByIdAsync(int id)
        {
            var statusNotification = await _statusNotificationRepository.GetByIdAsync(id);
            return statusNotification == null ? null : StatusNotificationMapper.ToResponseDto(statusNotification);
        }

        public async Task UpdateAsync(int statusNotificationId, StatusNotificationRequest statusNotificationRequest)
        {
            var entity = await _statusNotificationRepository.GetByIdAsync(statusNotificationId);
            if (entity != null)
            {
                // Update properties
                entity.StatusName = statusNotificationRequest.StatusName;
                await _statusNotificationRepository.UpdateAsync(entity);
            }
        }
    }
}
