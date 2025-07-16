using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IBloodRequestService
    {
        Task<IEnumerable<BloodRequestResponse>> GetAllAsync();
        Task<BloodRequestResponse?> GetByIdAsync(int requestId);
        Task<BloodRequestResponse> AddAsync(BloodRequestRequest request);
        Task<BloodRequestResponse?> UpdateAsync(int id, BloodRequestRequest request);
        Task DeleteAsync(int requestId);
    }
}
