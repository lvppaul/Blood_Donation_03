using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IBloodCompatibilityService
    {
        Task<BloodCompatibilityResponse> AddAsync(BloodCompatibilityRequest request);
        Task DeleteAsync(int bloodCompatibilityId);
        Task<BloodCompatibilityResponse?> GetByIdAsync(int bloodCompatibilityId);
        Task<BloodCompatibilityResponse?> UpdateAsync(int bloodCompatibilityId, BloodCompatibilityRequest request);
        Task<IEnumerable<BloodCompatibilityResponse>> GetAllAsync();
    }
}
