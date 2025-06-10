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

        public async Task DeleteDonorAvailabilityAsync(int id)
        {
            var donorAvailability = await _context.DonorAvailabilities.FindAsync(id);
            if (donorAvailability == null)
            {
                throw new Exception("Donor availability not found");
            }
            donorAvailability.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonorAvailability>> GetAllDonorAvailabilitiesAsync()
        {
            var donorAvailabilities = await _context.DonorAvailabilities.ToListAsync();

            return donorAvailabilities;
        }

        public async Task<DonorAvailability> GetDonorAvailabilityByIdAsync(int id)
        {
            var donorAvailability = await _context.DonorAvailabilities.FindAsync(id);
            if (donorAvailability == null)
            {
                return null;    
            }
            return donorAvailability;
        }

        public async Task<DonorAvailability> UpdateDonorAvailabilityAsync(DonorAvailability donorAvailability)
        {
            _context.DonorAvailabilities.Update(donorAvailability);
            await _context.SaveChangesAsync();
            return donorAvailability;
        }
    }
}
