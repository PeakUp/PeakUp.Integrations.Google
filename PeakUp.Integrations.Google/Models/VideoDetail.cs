using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class VideoDetail
    {
        [JsonProperty(PropertyName = "publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty(PropertyName = "channelId")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "thumbnails")]
        public Thumbnail Thumbnail { get; set; }

        [JsonProperty(PropertyName = "channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty(PropertyName = "liveBroadcastContent")]
        public string LiveBroadcastConent { get; set; }

        [JsonProperty(PropertyName = "resourceId")]
        public VideoIdentifier ResourceIdentifier { get; set; }

    }
}
