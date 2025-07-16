using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class StatusBloodRequestMapper
    {
        public static StatusBloodRequest ToEntity(StatusBloodRequestRequest request)
        {
            return new StatusBloodRequest
            {
                StatusName = request.StatusName,
                IsDeleted = false
            };
        }

        public static StatusBloodRequestResponse ToResponse(StatusBloodRequest entity)
        {
            return new StatusBloodRequestResponse
            {
                StatusRequestId = entity.StatusRequestId,
                StatusName = entity.StatusName,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}