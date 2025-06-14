using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IStatusBloodInventoryService
    {
        Task<IEnumerable<StatusBloodInventoryResponse>> GetAllAsync();
        Task<StatusBloodInventoryResponse?> GetByIdAsync(int id);
        Task<StatusBloodInventoryResponse> AddAsync(StatusBloodInventoryRequest statusBloodInventory);
        Task<StatusBloodInventoryResponse?> UpdateAsync(int statusBloodInventoryId, StatusBloodInventoryRequest statusBloodInventoryRequest);
        Task DeleteAsync(int id);
    }
}
