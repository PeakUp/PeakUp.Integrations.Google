using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Google.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Helpers
{
    public static class RequestUri
    {
        public static string GetRequestUrl(Base service, Api api, ApiVersion version, Endpoint endpoint)
        {
            return $"{service.GetDescription()}{api.GetDescription()}/{version.GetDescription()}/{endpoint.GetDescription()}";
        }

        public static string GetRequestUrl(Base service, Api api, Endpoint endpoint)
        {
            return $"{service.GetDescription()}{api.GetDescription()}/{endpoint.GetDescription()}";
        }

        public static string Append(this string baseString, Dictionary<string, string> parameters)
        {
            var builder = new StringBuilder(baseString);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    var key = builder.Length == baseString.Length ? "?" : "&";
                    builder.Append($"{key}{parameter.Key}={parameter.Value}");
                }
            }

            return builder.ToString();

        }
    }
}
