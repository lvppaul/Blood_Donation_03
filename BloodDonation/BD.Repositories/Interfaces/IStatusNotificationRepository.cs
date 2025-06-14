using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusNotificationRepository
    {
        Task<IEnumerable<StatusNotification>> GetAllAsync();
        Task<StatusNotification?> GetByIdAsync(int id);
        Task<StatusNotification> AddAsync(StatusNotification statusNotification);
        Task<StatusNotification> UpdateAsync(StatusNotification statusNotification);
        Task DeleteAsync(int id); // Soft delete: set IsDeleted = true


    }
}
