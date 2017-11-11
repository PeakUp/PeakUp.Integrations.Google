using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Channel
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "snippet")]
        public ChannelDetail Detail { get; set; }

        [JsonProperty(PropertyName = "statistics")]
        public ChannelStatistic Statistic { get; set; }

        [JsonProperty(PropertyName = "contentDetails")]
        public ChannelContentDetail ContentDetail { get; set; }



    }
}
