using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IDonorAvailabilityService
    {
        Task<IEnumerable<DonorAvailabilityResponse>> GetAllAsync();
        Task<DonorAvailabilityResponse?> GetByIdAsync(int id);
        Task<DonorAvailabilityResponse> AddAsync(DonorAvailabilityRequest donorAvailability);
        Task<DonorAvailabilityResponse?> UpdateAsync(int id, DonorAvailabilityRequest donorAvailability);
        Task DeleteAsync(int id);
    }
}
