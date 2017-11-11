using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Core.Helpers;
using PeakUp.Integrations.Google.Enums;
using PeakUp.Integrations.Google.Extensions;
using PeakUp.Integrations.Google.Helpers;
using PeakUp.Integrations.Google.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PeakUp.Integrations.Google.Services
{
    public class GoogleService
    {
        #region Defaults

        private const string DefaultDateFormat = "yyyy-MM-dd";

        #endregion

        private Credentials Credentials { get; set; }
        private Token Token { get; set; }
        public GoogleService(Credentials credentials)
        {
            if (credentials != null && credentials.Validate())
            {
                this.Credentials = credentials;
                VideoExtensions.Init(Credentials);
            }
            else
                throw new Exception("Credentials is not valid! All fields are required.");


        }

        /// <summary>
        /// You can use it for valid tokens that you have stored before.
        /// </summary>
        /// <param name="token">Token model</param>
        public void SetToken(Token token)
        {
            if (!token.Validate())
                throw new Exception("Token is not valid!");
            else
                this.Token = token;
        }

        /// <summary>
        /// Get current access token.
        /// </summary>
        /// <param name="token">Token model</param>
        public Token GetToken()
        {
            return this.Token;
        }

        /// <summary>
        /// Get Outh2 url for user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetOuth2Url(Outh2RequestBody request)
        {

            if (!request.Validate())
                throw new Exception("Outh2 Request Body is not valid!");

            var requestUrl = RequestUri.GetRequestUrl(Base.Accounts, Api.Outh2, ApiVersion.V2, Endpoint.Auth);

            #region Set Parameters

            var parameters = new Dictionary<string, string>()
            {
                { "scope" ,request.Scopes.Parse()},
                { "access_type" ,request.AccessType.GetDescription()},
                { "include_granted_scopes" ,request.IncludeGrantedScopes.ToString().ToLower()},
                { "state" ,request.State},
                { "redirect_uri" ,this.Credentials.RedirectUrl},
                { "response_type" ,request.ResponseType.GetDescription()},
                { "client_id" ,this.Credentials.ClientId}
            };

            #endregion

            requestUrl = requestUrl.Append(parameters);

            return requestUrl;

        }





        /// <summary>
        /// Get auth acccess token from google. (For enable scopes)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Token> Auth(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception("Code is not valid!");

            var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Auth, ApiVersion.V4, Endpoint.Token);

            #region Set Parameters

            var parameters = new Dictionary<string, string>()
            {
                { "code" ,code},
                { "client_id" ,this.Credentials.ClientId},
                { "client_secret" ,this.Credentials.ClientSecret},
                { "redirect_uri" ,this.Credentials.RedirectUrl},
                { "grant_type" ,GrantType.Code.GetDescription()}
            };

            #endregion

            requestUrl = requestUrl.Append(parameters);

            var response = await HttpHelper.Instance.PostAsync(requestUrl, "");

            var tokenObject = !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<Token>(response) : null;

            if (tokenObject != null)
            {
                this.Token = tokenObject;
                AnalyticsExtensions.Init(this.Credentials, this.Token);
            }

            return tokenObject;

        }

        /// <summary>
        /// Revoke access token. Returns true if token revoked successfully.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RevokeToken()
        {

            if (this.Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.Accounts, Api.Outh2, Endpoint.RevokeToken);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
            {
                { "token" ,this.Token.Value},
            };

                #endregion

                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                if (response == "{}")
                {
                    this.Token = null;
                    return true;
                }


                return false;

            }
            else
            {
                throw new Exception("Token not found!");
            }

        }


        /// <summary>
        /// Refresh access token.
        /// </summary>
        /// <returns></returns>
        public async Task<Token> RefreshToken()
        {

            if (this.Token != null)
            {

                if (!string.IsNullOrWhiteSpace(Token.RefreshToken))
                {
                    var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Outh2, ApiVersion.V4, Endpoint.RefreshToken);

                    #region Set Parameters

                    var parameters = new Dictionary<string, string>()
            {
                { "client_id" ,this.Credentials.ClientId},
                { "client_secret" ,this.Credentials.ClientSecret},
                { "refresh_token" ,this.Token.RefreshToken},
                { "grant_type" ,GrantType.RefreshToken.GetDescription()},
            };

                    #endregion

                    requestUrl = requestUrl.Append(parameters);

                    var response = await HttpHelper.Instance.PostAsync(requestUrl, "");

                    var tokenObject = !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<Token>(response) : null;

                    if (tokenObject != null)
                    {
                        this.Token = tokenObject;
                        AnalyticsExtensions.Init(this.Credentials, this.Token);
                    }

                    return tokenObject;
                }
                else
                {
                    throw new Exception("Refresh token not found!");
                }



            }
            else
            {
                throw new Exception("Token not found!");
            }

        }

        private async Task<TokenInformation> GetTokenInformation()
        {

            if (this.Token != null)
            {

                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Auth, ApiVersion.V1, Endpoint.TokenInfo);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
            {

                { "access_token" ,this.Token.Value}

            };



                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<TokenInformation>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }

        }


        /// <summary>
        /// If your access token is expired, it'll refresh it.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshIfTokenExpired()
        {
            if (Token != null)
            {
                var tokenInformation = await this.GetTokenInformation();
                if (tokenInformation != null && !tokenInformation.IsActive)
                {
                    if (this.Token.RefreshToken != null)
                    {
                        await this.RefreshToken();
                    }
                    else
                    {
                        throw new Exception("Refresh token not found!");
                    }
                }
            }
            else
            {
                throw new Exception("Token not found!");
            }
        }


        /// <summary>
        /// Get authenticated user youtube channels.
        /// </summary>
        /// <returns></returns>
        public async Task<GoogleApiResponse<Channel>> GetChannels()
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Channels);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "part" ,$"{Part.Snippet.GetDescription()},{Part.Statistics.GetDescription()},{Part.ContentDetails.GetDescription()}"},
                { "mine", "true" }

                };

                #endregion

                #region Set Headers

                var headers = new Dictionary<string, string>()
            {
                { "Authorization",$"{this.Token.Type} {this.Token.Value}"}
            };

                #endregion

                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl, headers);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Channel>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        /// <summary>
        /// Get authenticated user youtube channels.
        /// </summary>
        /// <param name="pageToken">Next or previous page token.</param>
        /// <returns></returns>

        public async Task<GoogleApiResponse<Channel>> GetChannels(string pageToken)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Channels);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "part" ,$"{Part.Snippet.GetDescription()}, {Part.Statistics.GetDescription()}"},
                { "mine", "true" },
                { "pageToken", pageToken }

                };

                #endregion

                #region Set Headers

                var headers = new Dictionary<string, string>()
            {
                { "Authorization",$"{this.Token.Type} {this.Token.Value}"}
            };

                #endregion

                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl, headers);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Channel>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }


        /// <summary>
        /// Get channel's videos.
        /// </summary>
        /// <param name="channelId">Youtube channel id.</param>
        /// <returns></returns>
        public async Task<GoogleApiResponse<Video<VideoIdentifier>>> GetChannelVideos(string channelId)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Search);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "order" ,"date"},
                { "part", Part.Snippet.GetDescription() },
                { "channelId" ,channelId},
                { "key" ,this.Credentials.ApiKey},
                { "type" ,"video"},

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Video<VideoIdentifier>>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        /// <summary>
        /// Get channel's videos.
        /// </summary>
        /// <param name="channelId">Youtube channel id.</param>
        /// <returns></returns>
        public async Task<GoogleApiResponse<Video<VideoIdentifier>>> GetChannelVideos(string channelId, DateTime publishedAfter, DateTime publishedBefore)
        {
            if (Token != null)
            {

                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Search);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "order" ,"date"},
                { "part", Part.Snippet.GetDescription() },
                { "channelId" ,channelId},
                { "key" ,this.Credentials.ApiKey},
                { "type" ,"video"},
                 { "publishedAfter" ,publishedAfter.ToString("yyyy-MM-ddTHH:mm:ssKZ")},
                { "publishedBefore" ,publishedBefore.ToString("yyyy-MM-ddTHH:mm:ssKZ")}

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Video<VideoIdentifier>>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        /// <summary>
        /// Get channel's videos.
        /// </summary>
        /// <param name="channelId">Youtube channel id.</param>
        /// <param name="pageToken">Next or previous page token.</param>
        /// <returns></returns>
        public async Task<GoogleApiResponse<Video<VideoIdentifier>>> GetChannelVideos(string channelId, string pageToken)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Search);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "order" ,"date"},
                { "part", Part.Snippet.GetDescription() },
                { "channelId" ,channelId},
                { "key" ,this.Credentials.ApiKey},
                { "pageToken" ,pageToken},
                { "type" ,"video"}

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Video<VideoIdentifier>>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        /// <summary>
        /// Get channel's videos.
        /// </summary>
        /// <param name="channelId">Youtube channel id.</param>
        /// <param name="pageToken">Next or previous page token.</param>
        /// <returns></returns>
        public async Task<GoogleApiResponse<Video<VideoIdentifier>>> GetChannelVideos(string channelId, string pageToken, DateTime publishedAfter, DateTime publishedBefore)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Search);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "order" ,"date"},
                { "part", Part.Snippet.GetDescription() },
                { "channelId" ,channelId},
                { "key" ,this.Credentials.ApiKey},
                { "pageToken" ,pageToken},
                { "type" ,"video"},
                { "publishedAfter" ,publishedAfter.ToString("yyyy-MM-ddTHH:mm:ssK")},
                { "publishedBefore" ,publishedBefore.ToString("yyyy-MM-ddTHH:mm:ssK")}

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Video<VideoIdentifier>>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        public async Task<GoogleApiResponse<PlaylistItem>> GetPlaylistItems(string playlistId)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.PlaylistItems);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "part", $"{Part.Snippet.GetDescription()},{Part.Status.GetDescription()},{Part.ContentDetails.GetDescription()}" },
                { "playlistId" ,playlistId},
                { "key" ,this.Credentials.ApiKey},


                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<PlaylistItem>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        public async Task<GoogleApiResponse<PlaylistItem>> GetPlaylistItems(string playlistId, DateTime startDate, DateTime endDate, bool includeLastVideo = false)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.PlaylistItems);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "part", $"{Part.Snippet.GetDescription()},{Part.Status.GetDescription()},{Part.ContentDetails.GetDescription()}" },
                { "playlistId" ,playlistId},
                { "key" ,this.Credentials.ApiKey},
                {"maxResults","50" },

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    var model = JsonConvert.DeserializeObject<GoogleApiResponse<PlaylistItem>>(response);
                    if (model != null)
                    {
                        var lastItemPublishDate = model.Items.LastOrDefault().VideoDetail?.PublishedAt;

                        if (model.Items.Where(t => t.VideoDetail.PublishedAt >= endDate).Count() > 0)
                        {

                            var videos = model.Items.Where(t => t.VideoDetail.PublishedAt <= startDate && t.VideoDetail.PublishedAt >= endDate).ToList();

                            while (videos.LastOrDefault()?.VideoDetail?.PublishedAt > endDate)
                            {
                                if (!string.IsNullOrWhiteSpace(model.NextPageToken))
                                {
                                    var newVideosResult = await this.GetPlaylistItems(playlistId, model.NextPageToken);
                                    if (newVideosResult != null && newVideosResult.Items?.Count > 0)
                                    {
                                        videos = videos.Concat(newVideosResult.Items).ToList();
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            var requestedVideos = videos.Where(t => t.VideoDetail.PublishedAt <= startDate && t.VideoDetail.PublishedAt >= endDate).ToList();

                            if (!includeLastVideo && requestedVideos.Count > 0)
                                requestedVideos.Remove(requestedVideos.FirstOrDefault());

                            model.Items = requestedVideos;
                            return model;

                        }
                        else
                        {
                            model.Items = new List<PlaylistItem>();
                            return model;
                        }

                    }
                }

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<PlaylistItem>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        public async Task<GoogleApiResponse<PlaylistItem>> GetPlaylistItems(string playlistId, string pageToken)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.PlaylistItems);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "part", $"{Part.Snippet.GetDescription()},{Part.Status.GetDescription()}" },
                { "playlistId" ,playlistId},
                { "key" ,this.Credentials.ApiKey},
                { "pageToken" ,pageToken},

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<PlaylistItem>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        private async Task<GoogleApiResponse<Video<VideoIdentifier>>> GetChannelsLastVideo(string channelId)
        {
            if (Token != null)
            {
                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Youtube, ApiVersion.V3, Endpoint.Search);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
                {

                { "order" ,"date"},
                { "part", Part.Snippet.GetDescription() },
                { "channelId" ,channelId},
                { "key" ,this.Credentials.ApiKey},
                { "type" ,"video"},
                {"maxResults","1" }

                };

                #endregion


                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<GoogleApiResponse<Video<VideoIdentifier>>>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }
        }

        /// <summary>
        /// Gets user google profile.
        /// </summary>
        /// <returns></returns>
        public async Task<Profile> Me()
        {

            if (this.Token != null)
            {

                var requestUrl = RequestUri.GetRequestUrl(Base.GoogleApis, Api.Plus, ApiVersion.V1, Endpoint.Me);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
            {

                { "client_id" ,this.Credentials.ClientId}

            };

                #endregion

                #region Set Headers

                var headers = new Dictionary<string, string>()
            {
                { "Authorization",$"{this.Token.Type} {this.Token.Value}"}
            };

                #endregion

                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl, headers);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<Profile>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }



        }

        private async Task<AnalyticsApiResponse> GetAnalytics(AnalyticsRequestBody request)
        {

            if (!request.Validate())
                throw new Exception("Request or token not valid!");

            if (this.Token != null)
            {

                var requestUrl = RequestUri.GetRequestUrl(Base.Content, Api.Analytics, ApiVersion.V1, Endpoint.Reports);

                #region Set Parameters

                var parameters = new Dictionary<string, string>()
            {

                { "metrics" ,request.ParsedMetrics},
                { "dimensions" ,request.ParsedDimensions},
                {"start-date", request.StartDate.ToString(DefaultDateFormat)},
                {"end-date", request.EndDate.ToString(DefaultDateFormat)},
                {"ids", "channel==MINE"},
                { "key", this.Credentials.ApiKey},
                //{ "filters",$"video=={request.VideoId}"}

            };

                if (!string.IsNullOrWhiteSpace(request.VideoId))
                {
                    parameters.Add("filters", $"video=={request.VideoId}");
                }

                #endregion

                #region Set Headers

                var headers = new Dictionary<string, string>()
            {
                { "Authorization",$"{this.Token.Type} {this.Token.Value}"}
            };

                #endregion

                requestUrl = requestUrl.Append(parameters);

                var response = await HttpHelper.Instance.GetAsync(requestUrl, headers);

                return !string.IsNullOrWhiteSpace(response) ? JsonConvert.DeserializeObject<AnalyticsApiResponse>(response) : null;

            }
            else
            {
                throw new Exception("Token not found!");
            }

        }

        public async Task<List<AudienceStatistic>> GetStatisticsByGender(AveragePeriod period, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Gender
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate
            }))?.ToModel<AudienceStatistic>();



        }

        public async Task<List<AudienceStatistic>> GetStatisticsByGender(AveragePeriod period, string videoId, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Gender
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate,
                VideoId = videoId
            }))?.ToModel<AudienceStatistic>();

        }

        public async Task<List<AudienceStatistic>> GetStatisticsByAgeGroup(AveragePeriod period, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.AgeGroup
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate
            }))?.ToModel<AudienceStatistic>();

        }

        public async Task<List<AudienceStatistic>> GetStatisticsByAgeGroup(AveragePeriod period, string videoId, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.AgeGroup
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate,
                VideoId = videoId
            }))?.ToModel<AudienceStatistic>();

        }

        public async Task<List<AudienceStatistic>> GetStatisticsByGenderWithAgeGroup(AveragePeriod period, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Gender,
                 Dimension.AgeGroup
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate
            }))?.ToModel<AudienceStatistic>();

        }

        public async Task<List<AudienceStatistic>> GetStatisticsByGenderWithAgeGroup(AveragePeriod period, string videoId, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Gender,
                 Dimension.AgeGroup
            };

            var metrics = new List<Metric>()
            {
                Metric.ViewerPercentage
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate,
                VideoId = videoId
            }))?.ToModel<AudienceStatistic>();

        }

        public async Task<List<ViewStatistic>> GetViews(AveragePeriod period, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Last30Days
            };

            var metrics = new List<Metric>()
            {
                Metric.View
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate
            }))?.ToModel<ViewStatistic>();

        }


        public async Task<List<ViewStatistic>> GetViews(AveragePeriod period, string videoId, bool includeLastVideo = false)
        {

            #region Defaults

            DateTime periodStartDate = DateTime.Now.AddDays(-30);
            DateTime periodEndDate = DateTime.Now;
            DateTime channelsLastVideoPublishDate = DateTime.Now;

            var dimensions = new List<Dimension>()
            {
                 Dimension.Last30Days
            };

            var metrics = new List<Metric>()
            {
                Metric.View
            };

            #endregion

            #region Get Channels & Last Video

            //Get user main channel for period start date & channels last video publishDate
            var channels = await this.GetChannels();
            if (channels != null && channels.Items != null && channels.Items.Count > 0)
            {
                var mainChannel = channels.Items.FirstOrDefault();
                periodStartDate = mainChannel.Detail.PublishedAt;

                var channelsLastVideo = await this.GetChannelsLastVideo(mainChannel.Id);

                if (channelsLastVideo != null && channelsLastVideo.Items != null && channelsLastVideo.Items.Count > 0)
                {
                    channelsLastVideoPublishDate = channelsLastVideo.Items.FirstOrDefault().VideoDetail.PublishedAt;
                }

            }

            #endregion

            periodEndDate = includeLastVideo ? DateTime.Now : channelsLastVideoPublishDate.AddDays(-1);
            periodStartDate = period == AveragePeriod.Monthly ? periodEndDate.AddDays(-30) : periodStartDate;


            return (await this.GetAnalytics(new AnalyticsRequestBody()
            {
                Dimensions = dimensions,
                EndDate = periodEndDate,
                Metrics = metrics,
                StartDate = periodStartDate,
                VideoId = videoId
            }))?.ToModel<ViewStatistic>();

        }


    }
}
