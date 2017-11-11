using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class ChannelDetail
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty(PropertyName = "thumbnails")]
        public Thumbnail Thumbnail { get; set; }

    }
}
