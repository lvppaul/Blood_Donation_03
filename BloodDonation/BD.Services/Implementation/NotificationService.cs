﻿using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;

namespace BD.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationResponse> AddAsync(NotificationRequest request)
        {
            var notificationEntity = NotificationMapper.ToEntity(request);
            await _notificationRepository.AddAsync(notificationEntity);
            return NotificationMapper.ToResponseDto(notificationEntity);
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new Exception("Notification not found");
            }

            await _notificationRepository.DeleteAsync(notification);
        }

        public async Task<IEnumerable<NotificationResponse>> GetAllAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(NotificationMapper.ToResponseDto);
        }

        public async Task<NotificationResponse?> GetByIdAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            return notification == null ? null : NotificationMapper.ToResponseDto(notification);
        }

        public async Task<IEnumerable<NotificationResponse>> GetByUserIdAsync(int userId)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId);
            return notifications.Select(NotificationMapper.ToResponseDto);
        }

        public async Task<NotificationResponse?> UpdateAsync(int notificationId, NotificationRequest request)
        {
            var entity = await _notificationRepository.GetByIdAsync(notificationId);
            if (entity != null)
            {
                // Update properties
                entity.Message = request.Message;
                entity.Type = request.Type;
                entity.SentAt = request.SentAt;
                entity.StatusNotificationId = request.StatusNotificationId;
                await _notificationRepository.UpdateAsync(entity);
                return NotificationMapper.ToResponseDto(entity);
            }
            return null;
        }

    }
}
