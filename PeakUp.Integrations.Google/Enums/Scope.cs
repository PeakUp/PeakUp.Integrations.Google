using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{
    public enum Scope
    {
        [Description("https://www.googleapis.com/auth/youtube")]
        Youtube,
        [Description("https://www.googleapis.com/auth/plus")]
        GooglePlus,
        [Description("https://www.googleapis.com/auth/yt-analytics")]
        YoutubeAnalytics,
        [Description("https://www.googleapis.com/auth/userinfo.email")]
        Emails,
        [Description("https://www.googleapis.com/auth/plus.me ")]
        Me,
        [Description("https://www.googleapis.com/auth/plus.profile.agerange.read")]
        AgeRange
    }
}
