using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class AnalyticsApiResponse
    {
        [JsonProperty(PropertyName ="kind")]
        public string Kind { get; set; }

        [JsonProperty(PropertyName = "columnHeaders")]
        public List<Column> Columns { get; set; }

        [JsonProperty(PropertyName ="rows")]
        public List<List<object>> Rows { get; set; }

    }
}
