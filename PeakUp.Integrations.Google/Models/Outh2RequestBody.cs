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
        public bool Validate()
        {
            if (Scopes == null || Scopes?.Count < 0)
                return false;            

            return true;

        }
       

    }
}
