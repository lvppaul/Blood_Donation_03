namespace BD.Repositories.Models.DTOs.Responses
{
    public class RoleResponse
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;
        public bool? IsDeleted { get; set; }
    }
}
