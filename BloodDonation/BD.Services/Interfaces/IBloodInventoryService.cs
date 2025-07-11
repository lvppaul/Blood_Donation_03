using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using BD.Services.Others;

namespace BD.Services.Interfaces
{
    public interface IBloodInventoryService
    {
        Task<IEnumerable<BloodInventoryResponse>> GetAllAsync();
        Task<BloodInventoryResponse?> GetByIdAsync(int id);
        Task<BloodInventoryResponse> AddAsync(BloodInventoryRequest post);
        Task<BloodInventoryResponse?> UpdateAsync(int id, BloodInventoryRequest post);
        Task DeleteAsync(int id);
        Task<IEnumerable<BloodInventoryResponse>> GetAllBloodInventoriesByBloodTypeAsync(string type);
        Task<int> TotalBloodInventoriesByBloodTypeAsync(string type);
        Task<IEnumerable<BloodTypeOverview>> GetBloodTypeOverviewAsync();
        Task<(IEnumerable<BloodInventoryResponse>, int TotalCount)> GetFilteredAsync(
     string searchTerm = null,
     string bloodType = null,
     int? facilityId = null,
     int pageNumber = 1,
     int pageSize = 10);
    }
}
