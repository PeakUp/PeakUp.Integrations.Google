using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Google.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Models
{
    public partial class AnalyticsRequestBody
    {

        public bool Validate()
        {
            return true;
        }
       
        public string ParsedDimensions
        {
            get
            {
                var builder = new StringBuilder();
                if (Dimensions != null && Dimensions.Count > 0)
                {
                    foreach (var dimension in Dimensions)
                    {
                        if (builder.Length == 0)
                            builder.Append(dimension.GetDescription());
                        else
                            builder.Append($",{dimension.GetDescription()}");
                    }
                }

                return builder.ToString();
            }
        }

        public string ParsedMetrics
        {
            get
            {
                var builder = new StringBuilder();
                if (Metrics != null && Metrics.Count > 0)
                {
                    foreach (var metric in Metrics)
                    {
                        if (builder.Length == 0)
                            builder.Append(metric.GetDescription());
                        else
                            builder.Append($",{metric.GetDescription()}");
                    }
                }

                return builder.ToString();
            }
        }

    }
}
