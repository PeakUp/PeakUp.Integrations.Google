using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class ChannelContentDetail
    {
        [JsonProperty("relatedPlaylists")]
        public RelatedPlaylist RelatedPlaylists { get; set; }
    }
}
