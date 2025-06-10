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
        Task<DonorAvailability> GetDonorAvailabilityByIdAsync(int id);
        Task<DonorAvailability> AddDonorAvailabilityAsync(DonorAvailability donorAvailability);
        Task<DonorAvailability> UpdateDonorAvailabilityAsync(DonorAvailability donorAvailability);
        Task DeleteDonorAvailabilityAsync(int id);
    }
}
