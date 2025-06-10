using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Interfaces
{
    public interface IMedicalFacilityRepository
    {
        Task<IEnumerable<MedicalFacility>> GetAllFacilitiesAsync();
        Task<MedicalFacility> GetFacilityByIdAsync(int id);
        Task<MedicalFacility> AddFacilityAsync(MedicalFacility facility);
        Task<MedicalFacility> UpdateFacilityAsync(MedicalFacility facility);
        Task DeleteFacilityAsync(int id);
    }
}
