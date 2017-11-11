using PeakUp.Integrations.Core.Extensions;
using PeakUp.Integrations.Google.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Helpers
{
    public static class ScopeParser
    {        

        /// <summary>
        /// Parse scopes for google apis.
        /// </summary>
        /// <param name="scopes">Key: Scope, Value: ScopeType</param>
        /// <returns></returns>
        public static string Parse(this Dictionary<Scope, ScopeType> scopes)
        {

            StringBuilder builder = new StringBuilder();

            if (scopes != null && scopes.Count > 0)
            {
                foreach (var scope in scopes)
                {
                    var scopeValue = scope.Key.GetDescription();

                    if (scope.Value != ScopeType.NoScopeType)
                    {
                        var scopeTypeValue = scope.Value.GetDescription();
                        builder.Append($"{scopeValue}.{scopeTypeValue} ");
                    }
                }
            }

            return builder.ToString();

        }

    }
}
