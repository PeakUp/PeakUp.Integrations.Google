using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class VideoStatistic
    {       

        public long ViewCount
        {
            get { return Convert.ToInt64(_viewCount); }
        }

        public long LikeCount
        {
            get { return Convert.ToInt64(_likeCount); }
        }

        public long DislikeCount
        {
            get { return Convert.ToInt64(_dislikeCount); }
        }

        public long FavoriteCount
        {
            get { return Convert.ToInt64(_favoriteCount); }
        }

        public long CommentCount
        {
            get { return Convert.ToInt64(_commentCount); }
        }


    }
}
