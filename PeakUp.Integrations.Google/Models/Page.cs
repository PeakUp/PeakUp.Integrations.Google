using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class Page
    {
        [JsonProperty(PropertyName = "totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty(PropertyName = "resultsPerPage")]
        public int ResultsPerPage { get; set; }
    }
}
