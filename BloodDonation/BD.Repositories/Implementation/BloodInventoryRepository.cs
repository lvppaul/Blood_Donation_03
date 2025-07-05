using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Implementation
{
    public class BloodInventoryRepository : IBloodInventoryRepository
    {
        private readonly BloodDonationDbContext _context;
        public BloodInventoryRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<BloodInventory> AddBloodInventoryAsync(BloodInventory bloodInventory)
        {
            await _context.BloodInventories.AddAsync(bloodInventory);
            await _context.SaveChangesAsync();

            return bloodInventory;
        }

        public async Task DeleteBloodInventoryAsync(BloodInventory bloodInventory)
        {
            bloodInventory.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloodInventory>> GetAllBloodInventoriesAsync()
        {
            return await _context.BloodInventories.Include(d=> d.Facility).Include(d => d.StatusInventory)
                .Where(bi => bi.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<BloodInventory?> GetBloodInventoryByIdAsync(int id)
        {
            return await _context.BloodInventories
                .FirstOrDefaultAsync(bi => bi.InventoryId == id && bi.IsDeleted != true);
        }

        public async Task<BloodInventory> UpdateBloodInventoryAsync(BloodInventory bloodInventory)
        {
            _context.BloodInventories.Update(bloodInventory);
            await _context.SaveChangesAsync();
            return bloodInventory;
        }
        public async Task<IEnumerable<BloodInventory>> GetAllBloodInventoriesByBloodTypeAsync(string type)
        {
            return await _context.BloodInventories
                .Include(d => d.Facility)
                .Include(d => d.StatusInventory)
                .Where(bi => bi.IsDeleted != true && bi.BloodType == type)
                .ToListAsync();
        }
    }
}
