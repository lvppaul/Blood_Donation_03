using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IBloodRequestRepository
    {
        Task<IEnumerable<BloodRequest>> GetAllBloodRequestsAsync();
        Task<BloodRequest> GetBloodRequestByIdAsync(int id);
        Task<BloodRequest> AddBloodRequestAsync(BloodRequest bloodRequest);
        Task<BloodRequest> UpdateBloodRequestAsync(BloodRequest bloodRequest);
        Task DeleteBloodRequestAsync(int id);
    }
}
