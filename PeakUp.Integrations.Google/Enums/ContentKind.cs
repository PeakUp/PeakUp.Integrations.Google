using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{
    public enum ContentKind
    {
        [Description("youtube#video")]
        Video,
        [Description("youtube#channel")]
        Channel
    }
}
