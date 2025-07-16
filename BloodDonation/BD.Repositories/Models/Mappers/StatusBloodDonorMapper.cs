using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class StatusBloodDonorMapper
    {
        public static StatusBloodDonor ToEntity(StatusBloodDonorRequest request)
        {
            return new StatusBloodDonor
            {
                StatusName = request.StatusName,
                IsDeleted = false
            };
        }

        public static StatusBloodDonorResponse ToResponseDto(StatusBloodDonor entity)
        {
            return new StatusBloodDonorResponse
            {
                StatusDonorId = entity.StatusDonorId,
                StatusName = entity.StatusName,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
