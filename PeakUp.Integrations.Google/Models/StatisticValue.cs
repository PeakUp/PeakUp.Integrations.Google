using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class StatisticValue
    {

        [Description("viewerPercentage")]
        public double Percentage { get; set; }

        [Description("views")]
        public int Value { get; set; }

    }
}
