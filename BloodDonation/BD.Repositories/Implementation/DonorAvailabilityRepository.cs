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
    public class DonorAvailabilityRepository : IDonorAvailabilityRepository
    {
        private readonly BloodDonationDbContext _context;
        public DonorAvailabilityRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<DonorAvailability> AddDonorAvailabilityAsync(DonorAvailability donorAvailability)
        {
            await _context.DonorAvailabilities.AddAsync(donorAvailability);
            await _context.SaveChangesAsync();

            return donorAvailability;
        }

        public async Task DeleteDonorAvailabilityAsync(DonorAvailability donorAvailability)
        {
            donorAvailability.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonorAvailability>> GetAllDonorAvailabilitiesAsync()
        {
            return await _context.DonorAvailabilities
                .Where(da => da.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<DonorAvailability?> GetDonorAvailabilityByIdAsync(int id)
        {
            return await _context.DonorAvailabilities
                .FirstOrDefaultAsync(da => da.AvailabilityId == id && da.IsDeleted != true);
        }

        public async Task<DonorAvailability?> GetLatestDonorAvailabilityByUserIdAsync(int userId)
        {
            return await _context.DonorAvailabilities
                .Where(da => da.UserId == userId && da.IsDeleted != true)
                .OrderByDescending(da => da.AvailableDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DonorAvailability> UpdateDonorAvailabilityAsync(DonorAvailability donorAvailability)
        {
            _context.DonorAvailabilities.Update(donorAvailability);
            await _context.SaveChangesAsync();
            return donorAvailability;
        }
    }
}
