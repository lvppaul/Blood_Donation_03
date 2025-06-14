using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;

namespace BD.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> AddAsync(UserRequest user);
        Task<UserResponse?> UpdateAsync(int userId, UserRequest user);
        Task DeleteAsync(int id);
    }
}
