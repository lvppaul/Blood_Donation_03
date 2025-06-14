namespace BD.Repositories.Models.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; } = null!;
        public string BloodType { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int RoleId { get; set; }
    }
}
