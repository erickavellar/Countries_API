using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Covid
    {
        public string Country { get; set; }

        public int TotalCases { get; set; }

        public int? TotalDeths { get; set; }

        public int? TotalRecovery { get; set; }

        public int? TotalTests { get; set; }

    }
}
