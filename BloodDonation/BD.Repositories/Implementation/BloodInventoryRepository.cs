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
            return await _context.BloodInventories.Include(d => d.Facility).Include(d => d.StatusInventory)
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


        public async Task<(List<BloodInventory>, int TotalCount)> GetFilteredBloodInventoriesAsync(
    string searchTerm = null,
    string bloodType = null,
    int? facilityId = null,
    int pageNumber = 1,
    int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var query = _context.BloodInventories
                .Include(d => d.Facility)
                .Include(d => d.StatusInventory)
                .Where(bi => bi.IsDeleted != true);


            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(bi =>
                    bi.InventoryId.ToString().ToLower().Contains(searchTerm.ToLower()) ||
                    bi.BloodType.ToLower().Contains(searchTerm.ToLower()) ||
                    bi.ComponentType.ToLower().Contains(searchTerm.ToLower()) ||
                    bi.Facility.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(bloodType))
            {
                query = query.Where(bi => bi.BloodType == bloodType);
            }

            if (facilityId.HasValue)
            {
                query = query.Where(bi => bi.Facility.FacilityId == facilityId.Value);
            }

            int totalCount = await query.CountAsync();

            query = query.OrderBy(bi => bi.InventoryId)
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

          
            var result = await query.ToListAsync();

            return (result, totalCount);
        }
    }
}
