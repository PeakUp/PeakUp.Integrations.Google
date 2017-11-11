using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class GoogleApiResponse<T>
    {

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public Page PageInfo { get; set; }

        [JsonProperty(PropertyName = "nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty(PropertyName = "prevPageToken")]
        public string PreviousPageToken { get; set; }

        [JsonProperty("items")]
        public List<T> Items { get; set; }

    }
}
