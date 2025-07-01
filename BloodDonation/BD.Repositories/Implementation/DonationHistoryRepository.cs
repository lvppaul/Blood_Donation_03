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

        public async Task DeleteDonationHistoryAsync(DonationHistory donationHistory)
        {
            donationHistory.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DonationHistory>> GetAllDonationHistoryAsync()
        {
            var histories = await _context.DonationHistories.ToListAsync();

            return histories;
        }        public async Task<DonationHistory?> GetDonationHistoryByIdAsync(int id)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .FirstOrDefaultAsync(dh => dh.DonationId == id && dh.IsDeleted != true);
        }

        public async Task<IEnumerable<DonationHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .Where(dh => dh.UserId == userId && dh.IsDeleted != true)
                .OrderByDescending(dh => dh.DonationDate)
                .ToListAsync();
        }

        public async Task<DonationHistory?> GetLatestDonationByUserIdAsync(int userId)
        {
            return await _context.DonationHistories
                .Include(dh => dh.Facility)
                .Where(dh => dh.UserId == userId && dh.IsDeleted != true)
                .OrderByDescending(dh => dh.DonationDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DonationHistory> UpdateDonationHistoryAsync(DonationHistory donationHistory)
        {
            _context.DonationHistories.Update(donationHistory);
            await _context.SaveChangesAsync();
            return donationHistory;
        }
    }
}
