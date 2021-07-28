using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class ProgressReport
    {
        public int PercentageComplete { get; set; } = 0;
        public List<Country> SitesDownloaded { get; set; } = new List<Country>();
        public List<Covid> SitesDownloaded2 { get; set; } = new List<Covid>();
    }
}
