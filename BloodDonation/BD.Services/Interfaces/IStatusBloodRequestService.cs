using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IStatusBloodRequestService
    {
        Task<IEnumerable<StatusBloodRequestResponse>> GetAllAsync();
        Task<StatusBloodRequestResponse?> GetByIdAsync(int id);
        Task<StatusBloodRequestResponse> AddAsync(StatusBloodRequestRequest statusRequestRequest);
        Task<StatusBloodRequestResponse?> UpdateAsync(int statusRequestId, StatusBloodRequestRequest statusRequestRequest);
        Task DeleteAsync(int id);
    }
}
