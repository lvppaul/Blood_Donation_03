using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IStatusBloodDonorService
    {
        Task<IEnumerable<StatusBloodDonorResponse>> GetAllAsync();
        Task<StatusBloodDonorResponse?> GetByIdAsync(int id);
        Task<StatusBloodDonorResponse> AddAsync(StatusBloodDonorRequest statusNotification);
        Task<StatusBloodDonorResponse> UpdateAsync(int statusDonorId, StatusNotificationRequest statusDonorRequest);
        Task DeleteAsync(int id); // Soft delete: set IsDeleted = true

    }
}
