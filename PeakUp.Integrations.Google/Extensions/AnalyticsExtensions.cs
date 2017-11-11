using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Google.Enums;
using PeakUp.Integrations.Google.Models;
using PeakUp.Integrations.Google.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Extensions
{
    public static class AnalyticsExtensions
    {

        private static Credentials Credentials { get; set; }
        private static Token Token { get; set; }

        public static void Init(Credentials credentials, Token token)
        {
            Credentials = credentials;
            Token = token;
        }

        public static async Task<double> Average(this List<ViewStatistic> viewStatistic)
        {

            if (Token == null || Credentials == null)
                throw new Exception("Token or credentials not found!");

            double average = 0;

            if (viewStatistic != null && viewStatistic.Count > 0)
            {

                var googleService = new GoogleService(Credentials);
                googleService.SetToken(Token);

                var channel = await googleService.GetChannels();
                if (channel != null && channel.Items.Count > 0)
                {
                    var startDate = DateTime.ParseExact(viewStatistic.FirstOrDefault().Date, "yyyy-MM-dd",CultureInfo.InvariantCulture);
                    var endDate = DateTime.ParseExact(viewStatistic.LastOrDefault().Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    var channelVideos = await googleService.GetChannelVideos(channel.Items.FirstOrDefault().Id, startDate,endDate);
                    if (channelVideos != null && channelVideos.PageInfo != null)
                    {
                        var totalVideos = channelVideos.PageInfo.TotalResults;
                        var totalViews = viewStatistic.Sum(t => t.Value);

                        average = totalViews / totalVideos;
                       

                    }
                }                

            }

            return average;
        }

        //TODO:Refactor
        public static List<T> ToModel<T>(this AnalyticsApiResponse response) where T : class, new()
        {

            var statistics = new List<T>();

            var props = new T().GetPropertiesWithDescription();

            if (props != null && props.Count > 0)
            {
                var responseDimensions = response?.Columns?.Where(t => t.ColumnType == "DIMENSION")?.Select(t => t.Name);
                var modelDimensions = props.Select(t => t.Value);



                var equalsDimensions = responseDimensions.Intersect(modelDimensions);


                if (equalsDimensions != null && equalsDimensions.Count() > 0)
                {

                    foreach (var row in response.Rows)
                    {

                        var statistic = new T();

                        foreach (var dimension in equalsDimensions)
                        {
                            var dimensionColumn = response?.Columns?.FirstOrDefault(t => t.Name == dimension);
                            var dimensionProperty = props.FirstOrDefault(t => t.Value == dimension);


                            if (dimensionColumn != null)
                            {
                                var dimenisonRowIndex = response.Columns.IndexOf(dimensionColumn);
                                var dimensionValue = row[dimenisonRowIndex];

                                statistic.SetPropertyValue(dimensionValue, dimensionProperty.Key);

                                var responseMetrics = response?.Columns?.Where(t => t.ColumnType == "METRIC").Select(t => t.Name);
                                var modelMetrics = props.Select(t => t.Value);

                                var equalsMetrics = responseMetrics.Intersect(modelMetrics);

                                if (equalsMetrics != null && equalsMetrics.Count() > 0)
                                {
                                    foreach (var metric in equalsMetrics)
                                    {
                                        var metricColumn = response.Columns.FirstOrDefault(t => t.Name == metric);
                                        var metricProperty = props.FirstOrDefault(t => t.Value == metric);
                                        var metricParseType = new T().GetTypeWithPropetyName(metricProperty.Key);

                                        if (metricColumn != null)
                                        {
                                            var metricRowIndex = response.Columns.IndexOf(metricColumn);
                                            var metricValue = row[metricRowIndex];

                                            statistic.SetPropertyValue(Convert.ChangeType(metricValue, metricParseType), metricProperty.Key);
                                        }

                                    }
                                }


                            }

                        }

                        statistics.Add(statistic);

                    }
                }

            }
            else
            {
                throw new Exception("No property in model!");
            }

            return statistics;
        }



    }
}
