using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Enums
{

    public enum Base
    {
        [Description("https://www.googleapis.com/")]
        GoogleApis,
        [Description("https://accounts.google.com/")]
        Accounts,
        [Description("https://content.googleapis.com/youtube/")]
        Content
    }

    public enum Api
    {
        [Description("o/oauth2")]
        Outh2,
        [Description("oauth2")]
        Auth,
        [Description("plus")]
        Plus,
        [Description("analytics")]
        Analytics,
        [Description("youtube")]
        Youtube
    }

    public enum Endpoint
    {
        [Description("auth")]
        Auth,
        [Description("token")]
        Token,
        [Description("people/me")]
        Me,
        [Description("reports")]
        Reports,
        [Description("tokeninfo")]
        TokenInfo,
        [Description("token")]
        RefreshToken,
        [Description("revoke")]
        RevokeToken,
        [Description("channels")]
        Channels,
        [Description("search")]
        Search,
        [Description("videos")]
        Videos,
        [Description("playlistItems")]
        PlaylistItems
    }

    public enum ApiVersion
    {
        [Description("v1")]
        V1,
        [Description("v2")]
        V2,
        [Description("v3")]
        V3,
        [Description("v4")]
        V4
    }

}
