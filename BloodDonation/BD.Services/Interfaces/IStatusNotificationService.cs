using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IStatusNotificationService
    {
        Task<IEnumerable<StatusNotificationResponse>> GetAllAsync();
        Task<StatusNotificationResponse?> GetByIdAsync(int id);
        Task<StatusNotificationResponse> AddAsync(StatusNotificationRequest statusNotificationRequest);
        Task<StatusNotificationResponse?> UpdateAsync(int statusNotificationId, StatusNotificationRequest statusNotificationRequest);
        Task DeleteAsync(int id);
    }
}
