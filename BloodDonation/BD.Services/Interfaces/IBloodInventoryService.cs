using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IBloodInventoryService
    {
        Task<IEnumerable<BloodInventoryResponse>> GetAllPostsAsync();
        Task<BloodInventoryResponse?> GetByIdAsync(int id);
        Task<BloodInventoryResponse> AddAsync(BloodInventoryRequest post);
        Task<BloodInventoryResponse?> UpdateAsync(BloodInventoryRequest post);
        Task DeleteAsync(int id);
    }
}
