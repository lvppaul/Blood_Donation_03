using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusNotificationRepository
    {
        Task<IEnumerable<StatusNotification>> GetAllAsync();
        Task<StatusNotification?> GetByIdAsync(int id);
        Task<StatusNotification> AddAsync(StatusNotification statusNotification);
        Task<StatusNotification> UpdateAsync(StatusNotification statusNotification);
        Task DeleteAsync(StatusNotification statusNotification); // Soft delete: set IsDeleted = true


    }
}
