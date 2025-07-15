using BD.Repositories.Interfaces;
using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Mappers;
using BD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BD.Services.Implementation
{
    public class StatusBloodInventoryService : IStatusBloodInventoryService
    {
        private readonly IStatusBloodInventoryRepository _statusBloodInventoryRepository;
        public StatusBloodInventoryService(IStatusBloodInventoryRepository statusBloodInventoryRepository)
        {
            _statusBloodInventoryRepository = statusBloodInventoryRepository;
        }
        public async Task<StatusBloodInventoryResponse> AddAsync(StatusBloodInventoryRequest statusNotification)
        {
            var entity = StatusBloodInventoryMapper.ToEntity(statusNotification);
            await _statusBloodInventoryRepository.AddAsync(entity);
            return StatusBloodInventoryMapper.ToResponse(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var statusBloodInventory = await _statusBloodInventoryRepository.GetByIdAsync(id);
            if (statusBloodInventory == null)
            {
                throw new Exception("Status Blood Inventory not found");
            }
            await _statusBloodInventoryRepository.DeleteAsync(statusBloodInventory);
        }

        public async Task<IEnumerable<StatusBloodInventoryResponse>> GetAllAsync()
        {
            var statusBloodInventories = await _statusBloodInventoryRepository.GetAllAsync();
            return statusBloodInventories.Select(StatusBloodInventoryMapper.ToResponse);
        }

        public async Task<StatusBloodInventoryResponse?> GetByIdAsync(int id)
        {
            var statusBloodInventory = await _statusBloodInventoryRepository.GetByIdAsync(id);
            return statusBloodInventory == null ? null : StatusBloodInventoryMapper.ToResponse(statusBloodInventory);
        }

        public async Task<StatusBloodInventoryResponse?> UpdateAsync(int statusBloodInventoryId, StatusBloodInventoryRequest statusBloodInventoryRequest)
        {
            var entity = await _statusBloodInventoryRepository.GetByIdAsync(statusBloodInventoryId);
            if (entity != null)
            {
                // Update properties
                entity.StatusName = statusBloodInventoryRequest.StatusName;
                await _statusBloodInventoryRepository.UpdateAsync(entity);
                return StatusBloodInventoryMapper.ToResponse(entity);
            }
            return null;
        }

        public async Task<List<SelectListItem>> GetStatusSelectListAsync()
        {
            try
            {
                var list = await _statusBloodInventoryRepository.GetAllAsync();

                if (list == null || !list.Any())
                {
                    return new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "No statuses available" }
                    };
                }

                var selectList = list.Where(s => s.IsDeleted != true)
                                    .Select(s => new SelectListItem
                                    {
                                        Value = s.StatusInventoryId.ToString(),
                                        Text = s.StatusName ?? "Unnamed Status"
                                    }).ToList();

                if (!selectList.Any())
                {
                    return new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "No active statuses available" }
                    };
                }

                selectList.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Select Status"
                });

                return selectList;
            }
            catch (Exception ex)
            {
                // Log the error if you have logging
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Error loading statuses" }
                };
            }
        }
    }
}
