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

        public async Task DeleteBloodRequestAsync(int id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest == null)
            {
                throw new Exception("Blood request not found");
            }
            bloodRequest.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloodRequest>> GetAllBloodRequestsAsync()
        {
            var bloodRequests = await _context.BloodRequests.ToListAsync();

            return bloodRequests;
        }

        public async Task<BloodRequest> GetBloodRequestByIdAsync(int id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest == null)
            {
                return null;
            }
            return bloodRequest;
        }

        public async Task<BloodRequest> UpdateBloodRequestAsync(BloodRequest bloodRequest)
        {
            _context.BloodRequests.Update(bloodRequest);
            await _context.SaveChangesAsync();
            return bloodRequest;
        }
    }
}
