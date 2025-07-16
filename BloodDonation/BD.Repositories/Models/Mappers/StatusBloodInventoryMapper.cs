using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class StatusBloodInventoryMapper
    {
        public static StatusBloodInventory ToEntity(StatusBloodInventoryRequest statusBloodInventoryRequest)
        {
            return new StatusBloodInventory
            {
                StatusName = statusBloodInventoryRequest.StatusName,
                IsDeleted = false
            };
        }

        public static StatusBloodInventoryResponse ToResponse(StatusBloodInventory statusBloodInventory)
        {
            return new StatusBloodInventoryResponse
            {
                StatusInventoryId = statusBloodInventory.StatusInventoryId,
                StatusName = statusBloodInventory.StatusName,
                IsDeleted = statusBloodInventory.IsDeleted
            };
        }
    }
}
