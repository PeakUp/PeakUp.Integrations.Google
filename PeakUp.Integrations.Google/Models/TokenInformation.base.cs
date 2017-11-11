using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class TokenInformation
    {        
        public bool IsActive
        {
            get { return string.IsNullOrEmpty(Error) && ExpiresIn != null ? true : false; }
        }

    }
}
