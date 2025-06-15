using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IMedicalFacilityService
    {
        Task<IEnumerable<MedicalFacilityResponse>> GetAllAsync();
        Task<MedicalFacilityResponse?> GetByIdAsync(int id);
        Task<MedicalFacilityResponse> AddAsync(MedicalFacilityRequest request);
        Task<MedicalFacilityResponse?> UpdateAsync(int id, MedicalFacilityRequest request);
        Task DeleteAsync(int id);
    }
}
