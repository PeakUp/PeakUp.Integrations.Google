using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public class Name
    {

        [JsonProperty(PropertyName = "familyName")]
        public string FamilyName { get; set; }

        [JsonProperty(PropertyName = "givenName")]
        public string GivenName { get; set; }

    }
}
