using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int notificationId);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<Notification> AddAsync(Notification notification);
        Task<Notification> UpdateAsync(Notification notification);
        Task DeleteAsync(Notification notification);
    }
}
