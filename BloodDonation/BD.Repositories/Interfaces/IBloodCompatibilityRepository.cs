using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IBloodCompatibilityRepository
    {
        Task<IEnumerable<BloodCompatibility>> GetAllAsync();
        Task<BloodCompatibility?> GetByIdAsync(int bloodCompatibilityId);
        Task<BloodCompatibility> AddAsync(BloodCompatibility bloodCompatibility);
        Task<BloodCompatibility> UpdateAsync(BloodCompatibility bloodCompatibility);
        Task DeleteAsync(BloodCompatibility bloodCompatibility);
    }
}
