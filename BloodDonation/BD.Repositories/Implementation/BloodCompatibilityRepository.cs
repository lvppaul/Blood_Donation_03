using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BD.Repositories.Implementation
{
    public class BloodCompatibilityRepository : IBloodCompatibilityRepository
    {
        private readonly BloodDonationDbContext _context;

        public BloodCompatibilityRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<BloodCompatibility> AddAsync(BloodCompatibility bloodCompatibility)
        {
            await _context.BloodCompatibilities.AddAsync(bloodCompatibility);
            await _context.SaveChangesAsync();
            return bloodCompatibility;
        }

        public async Task DeleteAsync(int bloodCompatibilityId)
        {
            var bloodCompatibility = await _context.BloodCompatibilities.FindAsync(bloodCompatibilityId);
            if (bloodCompatibility != null)
            {
                bloodCompatibility.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<BloodCompatibility>> GetAllAsync()
        {
            return await _context.BloodCompatibilities.Where(b => b.IsDeleted != true).ToListAsync();
        }

        public async Task<BloodCompatibility?> GetByIdAsync(int bloodCompatibilityId)
        {
            var bloodCompatibility = await _context.BloodCompatibilities.FindAsync(bloodCompatibilityId);
            return bloodCompatibility != null ? bloodCompatibility : null;
        }

        public async Task<BloodCompatibility> UpdateAsync(BloodCompatibility bloodCompatibility)
        {
            _context.BloodCompatibilities.Update(bloodCompatibility);
            await _context.SaveChangesAsync();
            return bloodCompatibility;
        }
    }
}
