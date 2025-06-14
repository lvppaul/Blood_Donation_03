using BD.Repositories.Models.Entities;

namespace BD.Repositories.Interfaces
{
    public interface IStatusBloodInventoryRepository
    {
        Task<IEnumerable<StatusBloodInventory>> GetAllAsync();
        Task<StatusBloodInventory?> GetByIdAsync(int status_inventory_id);
        Task<StatusBloodInventory> AddAsync(StatusBloodInventory status);
        Task<StatusBloodInventory> UpdateAsync(StatusBloodInventory status);
        Task DeleteAsync(StatusBloodInventory status);
    }
}
