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

        public async Task DeleteBloodInventoryAsync(int id)
        {
            var bloodInventory = await _context.BloodInventories.FindAsync(id);
            if (bloodInventory == null)
            {
                throw new Exception("Blood inventory not found");
            }
            bloodInventory.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloodInventory>> GetAllBloodInventoriesAsync()
        {
            var bloodInventories = await _context.BloodInventories.ToListAsync();

            return bloodInventories;
        }

        public async Task<BloodInventory> GetBloodInventoryByIdAsync(int id)
        {
            var bloodInventory = await _context.BloodInventories.FindAsync(id);
            if (bloodInventory == null)
            {
                return null;
            }
            return bloodInventory;
        }

        public async Task<BloodInventory> UpdateBloodInventoryAsync(BloodInventory bloodInventory)
        {
            _context.BloodInventories.Update(bloodInventory);
            await _context.SaveChangesAsync();
            return bloodInventory;
        }
    }
}
