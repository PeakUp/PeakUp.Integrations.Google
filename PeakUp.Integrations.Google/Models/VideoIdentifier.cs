using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class VideoIdentifier<T>
    {
        [JsonProperty(PropertyName ="kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "videoId")]
        public T VideoId { get; set; }
    }

    public class VideoIdentifier : VideoIdentifier<string>
    {
      
    }

}
