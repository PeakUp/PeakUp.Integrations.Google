using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class VideoStatistic
    {

        [JsonProperty(PropertyName = "viewCount")]
        private string _viewCount { get; set; }

        [JsonProperty(PropertyName = "likeCount")]
        private string _likeCount { get; set; }

        [JsonProperty(PropertyName = "dislikeCount")]
        private string _dislikeCount { get; set; }

        [JsonProperty(PropertyName = "favoriteCount")]
        private string _favoriteCount { get; set; }

        [JsonProperty(PropertyName = "commentCount")]
        private string _commentCount { get; set; }


    }
}
