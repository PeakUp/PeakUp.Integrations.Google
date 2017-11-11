using PeakUp.Integrations.Google.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class AnalyticsRequestBody
    {

        public List<Dimension> Dimensions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Metric> Metrics { get; set; }
        public string VideoId { get; set; }

    }
}
