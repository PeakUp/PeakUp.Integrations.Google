using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class Thumbnail
    {
        [JsonProperty(PropertyName = "default")]
        public Image Default { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public Image Medium { get; set; }

        [JsonProperty(PropertyName = "high")]
        public Image High { get; set; }
    }
}
