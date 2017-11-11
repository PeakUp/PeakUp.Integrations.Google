using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class ChannelStatistic
    {

        [JsonProperty(PropertyName = "viewCount")]
        private string _viewCount { get; set; }

        [JsonProperty(PropertyName = "commentCount")]
        private string _commentCount { get; set; }

        [JsonProperty(PropertyName = "subscriberCount")]
        private string _subscriberCount { get; set; }

        [JsonProperty(PropertyName = "hiddenSubscriberCount")]
        public bool HiddenSubscriberCount { get; set; }

        [JsonProperty(PropertyName = "videoCount")]
        private string _videoCount { get; set; }

    }
}
