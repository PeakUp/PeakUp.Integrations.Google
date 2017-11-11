using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Token
    {

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(Type))
                return false;
            else
                return true;
        }

    }
}
