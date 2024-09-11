using System.Collections.Generic;
using System.Windows.Documents;

namespace CitizenPortalSouthAfrica.Models
{
    public class ReportIssue
    {
        public int Id { get; set; }  //primary key
        public string Location { get; set; }    
        public string Category { get; set; }
        public string Description { get; set; }
        public List<byte[]> Files { get; set; } = new List<byte[]>();
    }
}
