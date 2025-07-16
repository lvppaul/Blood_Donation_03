using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(UserRequest userRequest)
        {
            return new User
            {
                Name = userRequest.Name,
                BloodType = userRequest.BloodType,
                Phone = userRequest.Phone,
                Email = userRequest.Email,
                Address = userRequest.Address,
                RoleId = userRequest.RoleId,
                Password = userRequest.Password, // In a real app, this should be hashed
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }
        public static UserResponse ToResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                BloodType = user.BloodType,
                Phone = user.Phone,
                Email = user.Email,
                Address = user.Address,
                IsDeleted = user.IsDeleted,
                CreatedAt = user.CreatedAt,
                Role = RoleMapper.ToResponse(user.Role)
            };
        }
    }
}
