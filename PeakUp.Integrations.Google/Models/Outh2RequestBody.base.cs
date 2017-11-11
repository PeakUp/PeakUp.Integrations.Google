using PeakUp.Integrations.Google.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Outh2RequestBody
    {

        public Dictionary<Scope, ScopeType> Scopes { get; set; }
        public AccessType AccessType { get; set; }
        public bool IncludeGrantedScopes { get; set; } = true;
        public string State { get; set; }
        public ResponseType ResponseType { get; set; }

    }
}
