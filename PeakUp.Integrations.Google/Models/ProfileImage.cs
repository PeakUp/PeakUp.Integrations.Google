using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class ProfileImage
    {
        [JsonProperty(PropertyName = "isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

    }
}
