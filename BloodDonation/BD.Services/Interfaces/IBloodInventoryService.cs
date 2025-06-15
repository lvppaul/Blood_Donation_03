using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IBloodInventoryService
    {
        Task<IEnumerable<BloodInventoryResponse>> GetAllAsync();
        Task<BloodInventoryResponse?> GetByIdAsync(int id);
        Task<BloodInventoryResponse> AddAsync(BloodInventoryRequest post);
        Task<BloodInventoryResponse?> UpdateAsync(int id, BloodInventoryRequest post);
        Task DeleteAsync(int id);
    }
}
