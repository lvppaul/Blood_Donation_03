using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class BloodCompatibilityMapper
    {
        public static BloodCompatibility ToEntity(BloodCompatibilityRequest request)
        {
            return new BloodCompatibility
            {
                RecipientBloodType = request.RecipientBloodType,
                DonorBloodType = request.DonorBloodType,
                ComponentType = request.ComponentType,
                IsCompatible = request.IsCompatible,
                IsDeleted = false
            };
        }

        public static BloodCompatibilityResponse ToResponse(BloodCompatibility entity)
        {
            return new BloodCompatibilityResponse
            {
                CompatibilityId = entity.CompatibilityId,
                RecipientBloodType = entity.RecipientBloodType,
                DonorBloodType = entity.DonorBloodType,
                ComponentType = entity.ComponentType,
                IsCompatible = entity.IsCompatible,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
