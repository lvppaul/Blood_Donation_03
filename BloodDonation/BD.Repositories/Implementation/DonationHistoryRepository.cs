using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.Entities;
using BD.Repositories.Models.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Implementation
{
    public class DonationHistoryRepository : IDonationHistoryRepository
    {
        private readonly BloodDonationDbContext _context;
        public DonationHistoryRepository(BloodDonationDbContext context)
        {
            _context = context;
        }
        public async Task<DonationHistory> AddDonationHistoryAsync(DonationHistory donationHistory)
        {
            await _context.DonationHistories.AddAsync(donationHistory);
            await _context.SaveChangesAsync();

            return donationHistory;
        }

        public async Task DeleteDonationHistoryAsync(int id)
        {
            var history = await _context.DonationHistories.FindAsync(id);
            if(history == null)
            {
                throw new Exception("Donation history not found");
            }
            history.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonationHistory>> GetAllDonationHistoryAsync()
        {
            var histories = await _context.DonationHistories.ToListAsync();

            return histories;
        }

        public async Task<DonationHistory> GetDonationHistoryByIdAsync(int id)
        {
            var history = await _context.DonationHistories.FindAsync(id);
            if (history == null)
            {
                return null;
            }
            return history;
        }

        public async Task<DonationHistory> UpdateDonationHistoryAsync(DonationHistory donationHistory)
        {
            _context.DonationHistories.Update(donationHistory);
            await _context.SaveChangesAsync();
            return donationHistory;
        }
    }
}
