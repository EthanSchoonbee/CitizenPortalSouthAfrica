using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Models
{
    public class ReportNode
    {
        public Report Report { get; set; }
        public ReportNode Left { get; set; }
        public ReportNode Right { get; set; }
        public ReportNode Parent { get; set; }
        public Constants.NodeColor Color { get; set; }

        public ReportNode(Report report)
        {
            Report = report;
            Color = Constants.NodeColor.Red; //New nodes are default red
        }
    }
}
