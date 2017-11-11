using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class Column
    {
        [JsonProperty(PropertyName ="name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "columnType")]
        public string ColumnType { get; set; }

        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get; set; }
    }
}
