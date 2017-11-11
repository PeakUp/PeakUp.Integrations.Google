using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class TokenInformation
    {
        [JsonProperty(PropertyName ="error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty(PropertyName = "issued_to")]
        public string IssuedTo { get; set; }

        [JsonProperty(PropertyName = "audience")]
        public string Audience { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "access_type")]
        public string AccessType { get; set; }

    }
}
