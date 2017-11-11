using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class RelatedPlaylist
    {
        [JsonProperty("likes")]
        public string Likes { get; set; }

        [JsonProperty("favorites")]
        public string Favorites { get; set; }

        [JsonProperty("uploads")]
        public string Uploads { get; set; }

        [JsonProperty("watchHistory")]
        public string WatchHistory { get; set; }

        [JsonProperty("watchLater")]
        public string WatchLater { get; set; }
    }
}
