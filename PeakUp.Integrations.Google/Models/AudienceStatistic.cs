using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class AudienceStatistic : StatisticValue
    {
        [Description("gender")]
        public string Gender { get; set; }

        [Description("ageGroup")]
        public string AgeGroup { get; set; }
    }
}
