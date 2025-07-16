using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.DTOs.Responses
{
    public class StatusBloodRequestResponse
    {
        public int StatusRequestId { get; set; }

        public string StatusName { get; set; } = null!;

        public bool? IsDeleted { get; set; }
    }
}
