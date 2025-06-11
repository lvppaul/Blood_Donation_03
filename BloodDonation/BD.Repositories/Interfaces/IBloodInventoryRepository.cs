using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IBloodInventoryRepository
    {
        Task<IEnumerable<BloodInventory>> GetAllBloodInventoriesAsync();
        Task<BloodInventory> GetBloodInventoryByIdAsync(int id);
        Task<BloodInventory> AddBloodInventoryAsync(BloodInventory bloodInventory);
        Task<BloodInventory> UpdateBloodInventoryAsync(BloodInventory bloodInventory);
        Task DeleteBloodInventoryAsync(int id);
    }
}
