using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IDonorAvailabilityRepository
    {
        Task<IEnumerable<DonorAvailability>> GetAllDonorAvailabilitiesAsync();
        Task<DonorAvailability?> GetDonorAvailabilityByIdAsync(int id);
        Task<DonorAvailability?> GetLatestDonorAvailabilityByUserIdAsync(int userId);
        Task<DonorAvailability> AddDonorAvailabilityAsync(DonorAvailability donorAvailability);
        Task<DonorAvailability> UpdateDonorAvailabilityAsync(DonorAvailability donorAvailability);
        Task DeleteDonorAvailabilityAsync(DonorAvailability donorAvailability);

        Task<IEnumerable<DonorAvailability>> GetAllAvailableDonorAsync();
        Task<IEnumerable<DonorAvailability>> GetAvailableDonorsByBloodTypeAsync(string bloodType);
        Task<IEnumerable<DonorAvailability>> SearchCompatibleDonorsAsync(string recipientBloodType);
    }
}
