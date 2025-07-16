using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusBloodDonorRepository
    {
        Task<IEnumerable<StatusBloodDonor>> GetAllAsync();
        Task<StatusBloodDonor?> GetByIdAsync(int status_donor_id);
        Task<StatusBloodDonor> AddAsync(StatusBloodDonor status);
        Task<StatusBloodDonor> UpdateAsync(StatusBloodDonor status);
        Task DeleteAsync(StatusBloodDonor status);
    }
}
