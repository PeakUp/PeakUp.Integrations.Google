using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{
    public enum Dimension
    {
        [Description("30DayTotals")]
        Last30Days,
        [Description("gender")]
        Gender,
        [Description("ageGroup")]
        AgeGroup
    }
}
