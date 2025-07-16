namespace BD.Repositories.Models.DTOs.Responses
{
    public class UserResponse
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string BloodType { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsDeleted { get; set; }

        public RoleResponse Role { get; set; } = null!;
    }
}
