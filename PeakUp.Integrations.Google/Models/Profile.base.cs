using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Profile
    {

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        [JsonProperty(PropertyName = "skills")]
        public string Skill { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string ProfileUrl { get; set; }

        [JsonProperty(PropertyName = "isPlusUser")]
        public bool IsGooglePlusUser { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "verified")]
        public bool IsVerifiedAccount { get; set; }

        [JsonProperty(PropertyName = "image")]
        public ProfileImage ProfileImage { get; set; }

        [JsonProperty(PropertyName = "name")]
        public Name Name { get; set; }

        [JsonProperty(PropertyName = "urls")]
        public List<Url> Urls { get; set; }

        [JsonProperty(PropertyName = "placesLived")]
        public List<Place> PlacesLived { get; set; }

        [JsonProperty(PropertyName ="emails")]
        public List<Email> Emails { get; set; }

    }
}
