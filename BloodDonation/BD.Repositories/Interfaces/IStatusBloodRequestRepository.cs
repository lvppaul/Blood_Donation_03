using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusBloodRequestRepository
    {
        Task<IEnumerable<StatusBloodRequest>> GetAllAsync();
        Task<StatusBloodRequest?> GetByIdAsync(int status_request_id);
        Task<StatusBloodRequest> AddAsync(StatusBloodRequest status);
        Task<StatusBloodRequest> UpdateAsync(StatusBloodRequest status);
        Task DeleteAsync(StatusBloodRequest status);
    }
}
