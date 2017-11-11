using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class ChannelStatistic
    {

        public long ViewCount
        {
            get { return Convert.ToInt64(_viewCount); }
        }

        public long CommentCount
        {
            get { return Convert.ToInt64(_commentCount); }
        }

        public long SubscriberCount
        {
            get { return Convert.ToInt64(_subscriberCount); }
        }

        public long VideoCount
        {
            get { return Convert.ToInt64(_videoCount); }
        }

    }
}
