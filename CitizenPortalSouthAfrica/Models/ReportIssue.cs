using System.Collections.Generic;
using System.Windows.Documents;

namespace CitizenPortalSouthAfrica.Models
{
    public class ReportIssue
    {
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<byte[]> Files { get; set; }
    }
}
