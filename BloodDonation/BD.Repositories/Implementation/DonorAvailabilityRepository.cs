using BD.Repositories.Interfaces;
using BD.Repositories.Models.Entities;
using Microsoft.EntityFrameworkCore;

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
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<DonorAvailability?> GetDonorAvailabilityByIdAsync(int id)
        {
            return await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .FirstOrDefaultAsync(da => da.AvailabilityId == id && da.IsDeleted != true);
        }

        public async Task<DonorAvailability?> GetLatestDonorAvailabilityByUserIdAsync(int userId)
        {
            return await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.UserId == userId && da.IsDeleted != true)
                .OrderByDescending(da => da.AvailableDate)
                .FirstOrDefaultAsync();
        }

        public async Task<DonorAvailability> UpdateDonorAvailabilityAsync(DonorAvailability donorAvailability)
        {
            _context.DonorAvailabilities.Update(donorAvailability);
            await _context.SaveChangesAsync();

            // Reload entity with navigation properties
            var updatedEntity = await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .FirstOrDefaultAsync(da => da.AvailabilityId == donorAvailability.AvailabilityId);

            return updatedEntity ?? donorAvailability;
        }

        public async Task<IEnumerable<DonorAvailability>> GetAllAvailableDonorAsync()
        {
            // First, get all donor availabilities
            var allDonorAvailabilities = await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.IsDeleted != true)
                .ToListAsync();

            // Then group by UserId and get the latest availability for each user
            var latestAvailabilityPerUser = allDonorAvailabilities
                .GroupBy(da => da.UserId)
                .Select(g => g.OrderByDescending(da => da.AvailableDate).First())
                .ToList();

            // Finally, filter only those with available status (StatusDonorId == 1)
            var availableDonors = latestAvailabilityPerUser
                .Where(da => da.StatusDonorId == 1)
                .ToList();

            return availableDonors;
        }

        public async Task<IEnumerable<DonorAvailability>> GetAllAvailableDonorSearchAsync()
        {
            // First, get all donor availabilities
            var allDonorAvailabilities = await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.IsDeleted != true)
                .ToListAsync();

            // Then group by UserId and get the latest availability for each user
            var latestAvailabilityPerUser = allDonorAvailabilities
                .GroupBy(da => da.UserId)
                .Select(g => g.OrderByDescending(da => da.AvailableDate).First())
                .ToList();

            // Finally, filter only those with available status (StatusDonorId == 1)
            var availableDonors = latestAvailabilityPerUser
                .Where(da => da.StatusDonorId == 1)
                .ToList();

            return availableDonors;
        }

        public async Task<IEnumerable<DonorAvailability>> GetAvailableDonorsByBloodTypeAsync(string bloodType)
        {
            // First, get all available donors with specific blood type
            var allAvailableDonors = await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.IsDeleted != true
                    && da.StatusDonorId == 1
                    && da.User.BloodType == bloodType)
                .ToListAsync();

            // Then group by UserId and get the latest availability for each user
            var latestAvailabilityPerUser = allAvailableDonors
                .GroupBy(da => da.UserId)
                .Select(g => g.OrderByDescending(da => da.AvailableDate).First())
                .ToList();

            return latestAvailabilityPerUser;
        }

        public async Task<IEnumerable<DonorAvailability>> SearchCompatibleDonorsAsync(string recipientBloodType)
        {
            // Blood compatibility logic - simplified version
            var compatibleBloodTypes = GetCompatibleBloodTypes(recipientBloodType);

            // First, get all compatible donors with their data
            var allCompatibleDonors = await _context.DonorAvailabilities
                .Include(da => da.User)
                .ThenInclude(u => u.Role)
                .Include(da => da.StatusDonor)
                .Where(da => da.IsDeleted != true
                    && da.StatusDonorId == 1
                    && compatibleBloodTypes.Contains(da.User.BloodType))
                .ToListAsync();

            // Then group by UserId and get the latest availability for each user
            var latestAvailabilityPerUser = allCompatibleDonors
                .GroupBy(da => da.UserId)
                .Select(g => g.OrderByDescending(da => da.AvailableDate).First())
                .OrderBy(da => da.User.BloodType == recipientBloodType ? 0 : 1) // Exact match first
                .ToList();

            return latestAvailabilityPerUser;
        }

        private List<string> GetCompatibleBloodTypes(string recipientBloodType)
        {
            // Basic blood compatibility rules
            return recipientBloodType switch
            {
                "A+" => new List<string> { "A+", "A-", "O+", "O-" },
                "A-" => new List<string> { "A-", "O-" },
                "B+" => new List<string> { "B+", "B-", "O+", "O-" },
                "B-" => new List<string> { "B-", "O-" },
                "AB+" => new List<string> { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" },
                "AB-" => new List<string> { "A-", "B-", "AB-", "O-" },
                "O+" => new List<string> { "O+", "O-" },
                "O-" => new List<string> { "O-" },
                _ => new List<string>()
            };
        }
    }
}
