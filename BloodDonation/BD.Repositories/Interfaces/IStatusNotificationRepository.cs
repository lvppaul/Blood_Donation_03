using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusNotificationRepository
    {
        Task<IEnumerable<StatusNotification>> GetAllAsync();
        Task<StatusNotification?> GetByIdAsync(int id);
        Task AddAsync(StatusNotification statusNotification);
        Task UpdateAsync(StatusNotification statusNotification);
        Task DeleteAsync(int id); // Soft delete: set IsDeleted = true


    }
}
