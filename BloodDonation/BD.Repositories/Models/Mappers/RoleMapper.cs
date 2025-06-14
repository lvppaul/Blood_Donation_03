using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;

namespace BD.Repositories.Models.Mappers
{
    public static class RoleMapper
    {
        public static Role ToEntity(string roleName)
        {
            return new Role
            {
                Name = roleName,
                IsDeleted = false
            };
        }
        public static RoleResponse ToResponse(Role entity)
        {
            return new RoleResponse
            {
                RoleId = entity.RoleId,
                Name = entity.Name,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
