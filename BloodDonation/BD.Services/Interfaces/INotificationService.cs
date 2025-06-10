using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface INotificationService
    {
        Task AddAsync(NotificationRequest request);
        Task DeleteAsync(int notificationId);
        Task<NotificationResponse?> GetByIdAsync(int notificationId);
        Task<IEnumerable<NotificationResponse>> GetByUserIdAsync(int userId);
        Task UpdateAsync(int notificationId, NotificationRequest request);
        Task<IEnumerable<NotificationResponse>> GetAllAsync();
    }
}
