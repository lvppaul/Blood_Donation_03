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
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private readonly BloodDonationDbContext _context;

        public BloodRequestRepository(BloodDonationDbContext context)
        {
            _context = context;
        }

        public async Task<BloodRequest> AddBloodRequestAsync(BloodRequest bloodRequest)
        {
            await _context.BloodRequests.AddAsync(bloodRequest);
            await _context.SaveChangesAsync();
            return bloodRequest;
        }

        public async Task<BloodRequest?> GetBloodRequestByIdAsync(int id)
        {
            return await _context.BloodRequests
                .FirstOrDefaultAsync(br => br.RequestId == id && br.IsDeleted != true);
        }

        public async Task<IEnumerable<BloodRequest>> GetAllBloodRequestsAsync()
        {
            return await _context.BloodRequests.Include(d=> d.User).Include(d => d.StatusRequest)
                .Where(br => br.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<BloodRequest> UpdateBloodRequestAsync(BloodRequest bloodRequest)
        {
            _context.BloodRequests.Update(bloodRequest);
            await _context.SaveChangesAsync();
            return bloodRequest;
        }

        public async Task DeleteBloodRequestAsync(BloodRequest bloodRequest)
        {
            bloodRequest.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
