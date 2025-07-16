using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Requests
{
    public class UpdateDonorStatusRequest
    {
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
