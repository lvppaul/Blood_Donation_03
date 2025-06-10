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
    public class MedicalFacilityRepository : IMedicalFacilityRepository
    {
        private readonly BloodDonationDbContext _context;
        public MedicalFacilityRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<MedicalFacility> AddFacilityAsync(MedicalFacility facility)
        {
            await _context.MedicalFacilities.AddAsync(facility);
            await _context.SaveChangesAsync();

            return facility;
        }

        public async Task DeleteFacilityAsync(int id)
        {
            var facility = await _context.MedicalFacilities.FindAsync(id);
            if (facility == null)
            {
                throw new Exception("Medical facility not found");
            }
            facility.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicalFacility>> GetAllFacilitiesAsync()
        {
            var facilities = await _context.MedicalFacilities.ToListAsync();

            return facilities;
        }

        public async Task<MedicalFacility> GetFacilityByIdAsync(int id)
        {
            var facility = await _context.MedicalFacilities.FindAsync(id);
            if (facility == null)
            {
                return null;
            }
            return facility;
        }

        public async Task<MedicalFacility> UpdateFacilityAsync(MedicalFacility facility)
        {
            _context.MedicalFacilities.Update(facility);
            await _context.SaveChangesAsync();
            return facility;
        }
    }
}
