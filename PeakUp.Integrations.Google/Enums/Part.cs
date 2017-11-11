using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{
    public enum Part
    {
        [Description("snippet")]
        Snippet,
        [Description("statistics")]
        Statistics,
        [Description("status")]
        Status,
        [Description("contentDetails")]
        ContentDetails
    }
}
