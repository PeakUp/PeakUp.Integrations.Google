using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{
    public enum GrantType
    {
        [Description("authorization_code")]
        Code,
        [Description("refresh_token")]
        RefreshToken
    }
}
