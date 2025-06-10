using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int notificationId);
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(int notificationId);
    }
}
