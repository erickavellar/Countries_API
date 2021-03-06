using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class RegionalBloc
    {
        public string acronym { get; set; }

        public string name { get; set; }

        public List<string> otherAcronyms { get; set; }

        public List<string> otherNames { get; set; }

        public string countryCode { get; set; }
    }
}
