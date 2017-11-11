using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Core.Helpers;
using PeakUp.Integrations.Google.Enums;
using PeakUp.Integrations.Google.Helpers;
using PeakUp.Integrations.Google.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Extensions
{
    public static class VideoExtensions
    {        

        private static Credentials Credentials { get; set; }

        public static void Init(Credentials credentials)
        {
            Credentials = credentials;
        }

        public static async Task<GoogleApiResponse<Video<VideoIdentifier>>> IncludeStatistics(this GoogleApiResponse<Video<VideoIdentifier>> videos)
        {

            if (videos != null && videos.Items.Count > 0)
            {
                foreach (var video in videos.Items)
                {
                    if (video.VideoIdentifier != null)
                    {
                        var videoId = ((VideoIdentifier)(object)video.VideoIdentifier).VideoId;

                        var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Videos);

                        #region Set Parameters

                        var parameters = new Dictionary<string, string>()
                        {
                            {"key", Credentials.ApiKey },
                            { "part", Part.Statistics.GetDescription() },
                            { "id", ((VideoIdentifier)(object)video.VideoIdentifier).VideoId}
                        };

                        #endregion

                        requestUrl = requestUrl.Append(parameters);

                        var response = await HttpHelper.Instance.GetAsync(requestUrl);

                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            var statistic = JsonConvert.DeserializeObject<GoogleApiResponse<Video<string>>>(response);
                            if (statistic != null && statistic.Items?.Count > 0)
                            {
                                video.Statistics = statistic.Items.FirstOrDefault().Statistics;
                            }
                        }

                    }
                }
            }

            return videos;

        }


        public static async Task<GoogleApiResponse<PlaylistItem>> IncludeStatistics(this GoogleApiResponse<PlaylistItem> playlistItems)
        {

            if (playlistItems != null && playlistItems.Items.Count > 0)
            {
                foreach (var playlistItem in playlistItems.Items)
                {
                    if (playlistItem.VideoDetail.ResourceIdentifier != null)
                    {
                        var videoId = ((VideoIdentifier)(object)playlistItem.VideoDetail.ResourceIdentifier).VideoId;

                        var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Videos);

                        #region Set Parameters

                        var parameters = new Dictionary<string, string>()
                        {
                            {"key", Credentials.ApiKey },
                            { "part", Part.Statistics.GetDescription() },
                            { "id", ((VideoIdentifier)(object)playlistItem.VideoDetail.ResourceIdentifier).VideoId}
                        };

                        #endregion

                        requestUrl = requestUrl.Append(parameters);

                        var response = await HttpHelper.Instance.GetAsync(requestUrl);

                        if (!string.IsNullOrWhiteSpace(response))
                        {
                            var statistic = JsonConvert.DeserializeObject<GoogleApiResponse<Video<string>>>(response);
                            if (statistic != null && statistic.Items?.Count > 0)
                            {
                                playlistItem.RelatedVideoStatistics = statistic.Items.FirstOrDefault().Statistics;
                            }
                        }

                    }
                }
            }

            return playlistItems;

        }

    }
}
