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

        public async Task DeleteFacilityAsync(MedicalFacility facility)
        {
            facility.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MedicalFacility>> GetAllFacilitiesAsync()
        {
            return await _context.MedicalFacilities
                .Where(f => f.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<MedicalFacility?> GetFacilityByIdAsync(int id)
        {
            return await _context.MedicalFacilities
                .FirstOrDefaultAsync(f => f.FacilityId == id && f.IsDeleted != true);
        }

        public async Task<MedicalFacility> UpdateFacilityAsync(MedicalFacility facility)
        {
            _context.MedicalFacilities.Update(facility);
            await _context.SaveChangesAsync();
            return facility;
        }
    }
}
