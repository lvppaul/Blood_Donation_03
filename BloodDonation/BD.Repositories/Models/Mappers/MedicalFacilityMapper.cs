using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.Mappers
{
    public class MedicalFacilityMapper
    {
        public static MedicalFacility ToEntity(MedicalFacilityRequest request)
        {
            return new MedicalFacility
            {
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                Email = request.Email,
                Description = request.Description,
                IsDeleted = false
            };
        }

        public static MedicalFacilityResponse ToResponse(MedicalFacility entity)
        {
            return new MedicalFacilityResponse
            {
                FacilityId = entity.FacilityId,
                Name = entity.Name,
                Address = entity.Address,
                Phone = entity.Phone,
                Email = entity.Email,
                Description = entity.Description,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
