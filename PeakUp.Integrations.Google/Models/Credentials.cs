using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class Credentials
    {

        public bool Validate()
        {
            if (!string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(ClientId) && !string.IsNullOrWhiteSpace(ClientSecret) && !string.IsNullOrWhiteSpace(RedirectUrl))
                return true;
            else
                return false;
        }

    }

   

}
