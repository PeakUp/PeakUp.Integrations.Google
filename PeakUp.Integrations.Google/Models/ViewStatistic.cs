using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class ViewStatistic : StatisticValue
    {
        [Description("30DayTotals")]
        public string Date { get; set; }
    }
}
