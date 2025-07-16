using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Enum
{
    public static class BloodTypeEnum
    {
        public const string A_POSITIVE = "A+";
        public const string A_NEGATIVE = "A-";
        public const string B_POSITIVE = "B+";
        public const string B_NEGATIVE = "B-";
        public const string AB_POSITIVE = "AB+";
        public const string AB_NEGATIVE = "AB-";
        public const string O_POSITIVE = "O+";
        public const string O_NEGATIVE = "O-";
        public static IEnumerable<string> GetAllBloodTypes()
        {
            return new List<string>
            {
                A_POSITIVE,
                A_NEGATIVE,
                B_POSITIVE,
                B_NEGATIVE,
                AB_POSITIVE,
                AB_NEGATIVE,
                O_POSITIVE,
                O_NEGATIVE
            };
        }
    }
}
