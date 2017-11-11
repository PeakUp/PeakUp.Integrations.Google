using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class VideoStatus
    {
        [JsonProperty("privacyStatus")]
        public string PrivacyStatus { get; set; }
    }
}
