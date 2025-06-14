using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class RoleResponse
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
