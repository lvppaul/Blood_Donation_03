using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class StatusBloodInventoryRepository : IStatusBloodInventoryRepository
    {
        private readonly BloodDonationDbContext _context;
        public StatusBloodInventoryRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<StatusBloodInventory> AddAsync(StatusBloodInventory status)
        {
            await _context.StatusBloodInventories.AddAsync(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task DeleteAsync(StatusBloodInventory status)
        {
            status.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StatusBloodInventory>> GetAllAsync()
        {
            return await _context.StatusBloodInventories
                  .Where(sn => sn.IsDeleted != true)
                  .ToListAsync();
        }

        public async Task<StatusBloodInventory?> GetByIdAsync(int status_inventory_id)
        {
            return await _context.StatusBloodInventories
            .FirstOrDefaultAsync(s => s.StatusInventoryId == status_inventory_id && s.IsDeleted != true);
        }

        public async Task<StatusBloodInventory> UpdateAsync(StatusBloodInventory status)
        {
            _context.StatusBloodInventories.Update(status);
            await _context.SaveChangesAsync();
            return status;
        }
    }
}
