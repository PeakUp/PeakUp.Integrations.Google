using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Video<T> where T :class
    {

        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "etag")]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "id")]
        public T VideoIdentifier { get; set; }

        [JsonProperty(PropertyName = "snippet")]
        public VideoDetail VideoDetail { get; set; }

        [JsonProperty(PropertyName = "statistics")]
        public VideoStatistic Statistics { get; set; }     

    }
}
